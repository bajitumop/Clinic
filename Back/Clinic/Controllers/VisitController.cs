namespace Clinic.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using DataAccess.Repositories;
    using Domain;
    using Services;
    using Support.Filters;

    using Microsoft.AspNetCore.Mvc;
    using Clinic.Models;

    [MustBeAuthorized]
    [UserPermission(UserPermission.CanVisitDoctor)]
    [ApiController]
    [Route("visits")]
    public class VisitController : BaseController
    {
        private readonly IUsersRepository usersRepository;
        private readonly IServicesRepository servicesRepository;
        private readonly IDoctorsRepository doctorsRepository;
        private readonly ISchedulesRepository schedulesRepository;
        private readonly IVisitsRepository visitsRepository;
        private readonly ScheduleService scheduleService;

        public VisitController(IUsersRepository usersRepository, IServicesRepository servicesRepository, IDoctorsRepository doctorsRepository, ISchedulesRepository schedulesRepository, IVisitsRepository visitsRepository, ScheduleService scheduleService)
        {
            this.usersRepository = usersRepository;
            this.servicesRepository = servicesRepository;
            this.doctorsRepository = doctorsRepository;
            this.schedulesRepository = schedulesRepository;
            this.visitsRepository = visitsRepository;
            this.scheduleService = scheduleService;
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> CreateVisit(long serviceId, long doctorId, DateTime dateTime)
        {
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

            if (doctor.Specialty != service.Specialty)
            {
                return this.Error("Доктор не может выполнять эту услугу", HttpStatusCode.BadRequest);
            }

            var schedule = await this.schedulesRepository.GetAsync(doctorId);
            var visits = (await this.visitsRepository.All(dateTime.Date, dateTime.Date.AddDays(1))).ToArray();

            var records = this.scheduleService.GetDoctorVisitsByDate(schedule, dateTime.Date, visits).ToArray();
            var record = records.FirstOrDefault(r => r.DateTime == dateTime && r.Status == VisitStatus.Opened);
            if (record == null || record.Status != VisitStatus.Opened)
            {
                return this.Error("Невозможно записаться на указанное время");
            }
            
            await this.visitsRepository.Create(this.GetUser().Username, serviceId, doctorId, dateTime);
            return this.Success();
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Visits(DateTime? from = null, DateTime? to = null)
        {
            var fromDate = from ?? DateTime.UtcNow;
            var toDate = to ?? DateTime.MaxValue;

            if (from > to)
            {
                var temp = fromDate;
                fromDate = toDate;
                toDate = temp;
            }

            var username = this.GetUser()?.Username;
            var visits = await this.visitsRepository.ByUser(username, fromDate, toDate);
            var doctors = await this.doctorsRepository.All();
            var services = await this.servicesRepository.All();
            var visitModels = visits.Select(v =>
            {
                var doctor = doctors.Single(d => d.Id == v.DoctorId);
                var service = services.Single(s => s.Id == v.ServiceId);
                var result = new VisitModel
                {
                    Id = v.Id,
                    DateTime = v.DateTime,
                    DoctorId = v.DoctorId,
                    ServiceId = v.ServiceId,
                    DoctorFirstName = doctor.FirstName,
                    DoctorSecondName = doctor.SecondName,
                    DoctorThirdName = doctor.ThirdName,
                    Specialty = doctor.Specialty,
                    ServiceDescription = service.Description
                };
                return result;
            });
            return this.Success(visitModels.OrderBy(v => v.DateTime).ToArray());
        }

        [HttpDelete, Route("")]
        public async Task<IActionResult> Delete(long id)
        {
            var username = this.GetUser()?.Username;
            await this.visitsRepository.Delete(username, id);
            return Success();
        }
    }
}
