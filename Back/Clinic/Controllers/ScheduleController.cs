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
    }
}
