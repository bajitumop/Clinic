namespace Clinic.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Clinic.DataAccess.Repositories;
    using Clinic.Models.Schedules;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("schedules")]
    public class ScheduleController : BaseController
    {
        private readonly ISchedulesRepository schedulesRepository;
        private readonly IMapper mapper;

        public ScheduleController(ISchedulesRepository schedulesRepository, IMapper mapper)
        {
            this.schedulesRepository = schedulesRepository;
            this.mapper = mapper;
        }

        [HttpGet, Route("table-by-week")]
        public async Task<IActionResult> ByWeek(long doctorId, long specialtyId)
        {
            var schedule = await this.schedulesRepository.GetAsync(doctorId, specialtyId);
            if (schedule == null)
            {
                return this.Error("Расписание не найдено для указанных id доктора и специальности", HttpStatusCode.NotFound);
            }

            return this.Success(this.mapper.Map<ScheduleByWeekModel>(schedule));
        }
    }
}
