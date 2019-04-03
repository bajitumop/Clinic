namespace Clinic.Controllers
{
    using System.Collections.Generic;
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
            var specialtiesDictionary = (await this.schedulesRepository.All())
                .GroupBy(schedule => schedule.DoctorId)
                .ToDictionary(g => g.Key, g => g.Select(s => s.Specialty).ToArray());

            var result = doctors.Select(doctor =>
                {
                    var doctorModel = this.mapper.Map<DoctorShortModel>(doctor);
                    doctorModel.Specialties = specialtiesDictionary[doctorModel.Id];
                    return doctorModel;
                })
                .ToArray();
            return this.Success(result);
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var doctor = await this.doctorsRepository.GetAsync(id);
            if (doctor == null)
            {
                return this.Error("Доктор с указанным id не найден в базе", HttpStatusCode.NotFound);
            }

            var specialties = (await this.schedulesRepository.GetByDoctorAsync(id)).Select(s => s.Specialty).ToArray();
            var doctorModel = this.mapper.Map<DoctorModel>(doctor);
            doctorModel.Specialties = specialties;
            return this.Success(doctorModel);
        }

        [HttpGet, Route("by-specialty")]
        public async Task<IActionResult> GetBySpecialty(string specialty)
        {
            var doctors = await this.doctorsRepository.GetBySpecialtyAsync(specialty);
            var result = new List<DoctorShortModel>();
            foreach (var doctor in doctors)
            {
                var model = this.mapper.Map<DoctorShortModel>(doctor);
                model.Specialties = (await this.schedulesRepository.GetByDoctorAsync(doctor.Id))
                    .Select(x => x.Specialty)
                    .Distinct()
                    .ToArray();
                result.Add(model);
            }
            return this.Success(result);
        }
    }
}
