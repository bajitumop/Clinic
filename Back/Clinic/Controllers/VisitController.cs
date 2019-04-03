namespace Clinic.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using DataAccess.Repositories;
    using Domain;
    using Support.Filters;

    using Microsoft.AspNetCore.Mvc;

    [MustBeAuthorized]
    [UserPermission(UserPermission.CanVisitDoctor)]
    [ApiController]
    [Route("visits")]
    [UserPermission(UserPermission.CanVisitDoctor)]
    public class VisitController : BaseController
    {
        private readonly IUsersRepository usersRepository;
        private readonly IServicesRepository servicesRepository;
        private readonly IDoctorsRepository doctorsRepository;
        private readonly ISchedulesRepository schedulesRepository;
        private readonly IVisitsRepository visitsRepository;

        public VisitController(IUsersRepository usersRepository, IServicesRepository servicesRepository, IDoctorsRepository doctorsRepository, ISchedulesRepository schedulesRepository, IVisitsRepository visitsRepository)
        {
            this.usersRepository = usersRepository;
            this.servicesRepository = servicesRepository;
            this.doctorsRepository = doctorsRepository;
            this.schedulesRepository = schedulesRepository;
            this.visitsRepository = visitsRepository;
        }

        /*[HttpPost, Route("create")]
        public async Task<IActionResult> CreateVisit([FromQuery]long serviceId, [FromQuery]long doctorId, [FromQuery]DateTime dateTime)
        {
            throw new NotImplementedException();
            var doctor = await this.doctorsRepository.GetAsync(doctorId);
            if (doctor == null)
            {
                return this.Error("Указанный доктор отсутствует в базе", HttpStatusCode.NotFound);
            }

            var service = await this.servicesRepository.GetAsync(serviceId);
            if (service == null)
            {
                return this.Error("Указанная услуга отсутствует в базе", HttpStatusCode.NotFound);
            }

            if (doctor.Schedules.Select(s => s.SpecialtyId).Contains(service.Specialty.Id))
            {
                return this.Error("Доктор не может выполнять эту услугу", HttpStatusCode.BadRequest);
            }

            

            var visit = new Visit { User = this.GetUser(), Service = service, Doctor = doctor, DateTime = dateTime };
            this.visitsRepository.Create(visit);
            await this.visitsRepository.SaveChangesAsync();
            return this.Success();
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Visits(DateTime? from = null, DateTime? to = null)
        {
            var fromDate = from ?? DateTime.MinValue;
            var toDate = to ?? DateTime.MaxValue;

            if (from > to)
            {
                var temp = from;
                from = to;
                to = temp;
            }

            var username = this.GetUser()?.Username;
            var visits = await this.visitsRepository.FromRange(username, fromDate, toDate);
            return this.Success(visits.ToArray());
        }*/
    }
}
