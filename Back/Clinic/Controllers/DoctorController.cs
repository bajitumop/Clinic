namespace Clinic.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;
    using Clinic.Models.Doctors;

    using Microsoft.AspNetCore.Mvc;

    [Route("doctors")]
    [ApiController]
    public class DoctorController : BaseController
    {
        private readonly IDoctorsRepository doctorsRepository;

        public DoctorController(IDoctorsRepository doctorsRepository)
        {
            this.doctorsRepository = doctorsRepository;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await this.doctorsRepository.All();
            return this.Success(doctors.Select(this.MapToShortModel).ToArray());
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var doctor = await this.doctorsRepository.GetAsync(id);
            if (doctor == null)
            {
                return this.Error("Доктор с указанным id не найден в базе", HttpStatusCode.NotFound);
            }

            return this.Success(this.MapToFullModel(doctor));
        }

        [HttpGet, Route("by-specialty")]
        public async Task<IActionResult> GetBySpecialty(long specialtyId)
        {
            var doctors = await this.doctorsRepository.GetBySpecialty(specialtyId);
            return this.Success(doctors.Select(this.MapToShortModel).ToArray());
        }

        private DoctorShortModel MapToShortModel(Doctor doctor)
        {
            return new DoctorShortModel
                       {
                           Id = doctor.Id,
                           FirstName = doctor.FirstName,
                           SecondName = doctor.SecondName,
                           ThirdName = doctor.ThirdName,
                           Positions = doctor.Positions,
                           ImageId = doctor.ImageId
                       };
        }

        private DoctorModel MapToFullModel(Doctor doctor)
        {
            return new DoctorModel
                       {
                           Id = doctor.Id,
                           FirstName = doctor.FirstName,
                           SecondName = doctor.SecondName,
                           ThirdName = doctor.ThirdName,
                           Positions = doctor.Positions,
                           ImageId = doctor.ImageId,
                           Info = doctor.Info,
                           Room = doctor.Room
                       };
        }
    }
}
