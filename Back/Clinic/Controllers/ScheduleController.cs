using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Domain;
using Clinic.Models.Schedules;
using Clinic.Services;

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
        private readonly ScheduleService scheduleService;
        private readonly IVisitsRepository visitsRepository;

        public ScheduleController(ISchedulesRepository schedulesRepository, IMapper mapper, ScheduleService scheduleService, 
            IDoctorsRepository doctorsRepository, IVisitsRepository visitsRepository)
        {
            this.schedulesRepository = schedulesRepository;
            this.mapper = mapper;
            this.scheduleService = scheduleService;
            this.doctorsRepository = doctorsRepository;
            this.visitsRepository = visitsRepository;
        }

        [HttpGet, Route("")]
        public async Task<VisitStatusInfoModel[]> GetDoctorSchedule(int doctorId)
        {
            var now = DateTime.UtcNow.Date;
            const int days = 14;
            var schedule = await this.schedulesRepository.GetAsync(doctorId);
            var visits = (await this.visitsRepository.All(now, now.AddDays(days))).ToArray();

            var records = Enumerable.Range(0, days)
                .SelectMany(i => this.scheduleService.GetDoctorVisitsByDate(schedule, now.AddDays(i), visits))
                .ToArray();

            PopulateRecords(records);
            return records;
        }

        private void PopulateRecords(VisitStatusInfoModel[] records)
        {
            var random = new Random();
            foreach (var statusInfo in records)
            {
                if (random.Next() % 2 == 0)
                {
                    //statusInfo.Status = VisitStatus.Closed;
                }
            }
        }

    }
}
