namespace Clinic.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Clinic.DataAccess.Repositories;
    using Clinic.Models.Doctors;

    using Microsoft.AspNetCore.Mvc;

    [Route("doctors")]
    [ApiController]
    public class DoctorController : BaseController
    {
        private readonly IDoctorsRepository doctorsRepository;
        private readonly IMapper mapper;

        public DoctorController(IDoctorsRepository doctorsRepository, IMapper mapper)
        {
            this.doctorsRepository = doctorsRepository;
            this.mapper = mapper;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await this.doctorsRepository.All();
            return this.Success(doctors.Select(this.mapper.Map<DoctorShortModel>).ToArray());
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
        public async Task<IActionResult> GetBySpecialty(long specialtyId)
        {
            var doctors = await this.doctorsRepository.GetBySpecialtyAsync(specialtyId);
            return this.Success(doctors.Select(this.mapper.Map<DoctorShortModel>).ToArray());
        }
    }
}
