namespace Clinic.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataAccess.Repositories;
    using Models.Doctors;

    using Microsoft.AspNetCore.Mvc;

    [Route("doctors")]
    [ApiController]
    public class DoctorController : BaseController
    {
        private readonly IDoctorsRepository doctorsRepository;
        private readonly ISchedulesRepository schedulesRepository;
        private readonly IMapper mapper;

        public DoctorController(IDoctorsRepository doctorsRepository, IMapper mapper, ISchedulesRepository schedulesRepository)
        {
            this.doctorsRepository = doctorsRepository;
            this.mapper = mapper;
            this.schedulesRepository = schedulesRepository;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await this.doctorsRepository.All();
            return this.Success(doctors.Select(d => this.mapper.Map<DoctorModel>(d)));
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var doctor = await this.doctorsRepository.GetAsync(id);
            if (doctor == null)
            {
                return this.Error("Доктор с указанным id не найден в базе", HttpStatusCode.NotFound);
            }

            return this.Success(this.mapper.Map<DoctorModel>(doctor));
        }

        [HttpGet, Route("by-specialty")]
        public async Task<IActionResult> GetBySpecialty(string specialty)
        {
            var doctors = await this.doctorsRepository.GetBySpecialtyAsync(specialty);
            return this.Success(doctors.Select(d => this.mapper.Map<DoctorModel>(d)));
        }
    }
}
