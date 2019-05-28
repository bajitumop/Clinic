using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Domain;
using Clinic.Models.Schedules;

namespace Clinic.Controllers
{
    using AutoMapper;

    using DataAccess.Repositories;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("schedules")]
    public class ScheduleController : BaseController
    {
        private readonly ISchedulesRepository schedulesRepository;
        private readonly IDoctorsRepository doctorsRepository;
        private readonly IMapper mapper;

        public ScheduleController(ISchedulesRepository schedulesRepository, IMapper mapper, IDoctorsRepository doctorsRepository)
        {
            this.schedulesRepository = schedulesRepository;
            this.mapper = mapper;
            this.doctorsRepository = doctorsRepository;
        }

        [HttpGet, Route("")]
        public async Task<VisitStatusInfoModel[]> GetDoctorSchedule(int doctorId)
        {
            var schedule = await this.schedulesRepository.GetAsync(doctorId);
            var now = DateTime.UtcNow.Date;
            var records = Enumerable.Range(0, 14)
                .SelectMany(i => GetVisitStatuses(schedule, now.AddDays(i), TimeSpan.FromMinutes(10)))
                .ToArray();

            await PopulateRecords(records);
            return records;
        }

        private async Task PopulateRecords(VisitStatusInfoModel[] records)
        {
            var random = new Random();
            foreach (var statusInfo in records)
            {
                if (random.Next() % 2 == 0)
                {
                    statusInfo.Status = VisitStatus.Closed;
                }
            }

            await Task.CompletedTask;
        }

        private IEnumerable<VisitStatusInfoModel> GetVisitStatuses(Schedule schedule, DateTime visitDate, TimeSpan visitDuration)
        {
            TimeSpan? startTime = null;
            TimeSpan? endTime = null;

            switch (visitDate.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    startTime = schedule.MondayStart;
                    endTime = schedule.MondayEnd;
                    break;
                case DayOfWeek.Tuesday:
                    startTime = schedule.TuesdayStart;
                    endTime = schedule.TuesdayEnd;
                    break;
                case DayOfWeek.Wednesday:
                    startTime = schedule.WednesdayStart;
                    endTime = schedule.WednesdayEnd;
                    break;
                case DayOfWeek.Thursday:
                    startTime = schedule.ThursdayStart;
                    endTime = schedule.ThursdayEnd;
                    break;
                case DayOfWeek.Friday:
                    startTime = schedule.FridayStart;
                    endTime = schedule.FridayEnd;
                    break;
                case DayOfWeek.Saturday:
                    startTime = schedule.SaturdayStart;
                    endTime = schedule.SaturdayEnd;
                    break;
            }

            if (!startTime.HasValue || !endTime.HasValue)
            {
                return Enumerable.Empty<VisitStatusInfoModel>();
            }

            var visitsCount = (int)((endTime.Value - startTime.Value).Ticks / visitDuration.Ticks);
            return Enumerable
                .Range(0, visitsCount)
                .Select(i => new VisitStatusInfoModel
                {
                    DateTime = visitDate.Add(TimeSpan.FromTicks(visitDuration.Ticks * i)),
                    Status = VisitStatus.Opened
                });
        }
    }
}
