namespace Clinic.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataAccess.Repositories;
    using Models.Schedules;

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

        /*[HttpGet, Route("table-by-week")]
        public async Task<IActionResult> ByWeek(long doctorId, string specialty)
        {
            var schedule = await this.schedulesRepository.GetAsync(doctorId, specialty);
            if (schedule == null)
            {
                return this.Error("Расписание не найдено для указанных id доктора и специальности", HttpStatusCode.NotFound);
            }

            return this.Success(this.mapper.Map<ScheduleByWeekModel>(schedule));
        }

        [HttpGet, Route("by-date")]
        public async Task<IActionResult> ByDate(long doctorId, long specialtyId, DateTime dateTime)
        {
            throw new NotImplementedException();
        }*/

    }
}
