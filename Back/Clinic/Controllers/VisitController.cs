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
            var fromDate = from ?? DateTime.MinValue;
            var toDate = to ?? DateTime.MaxValue;

            if (from > to)
            {
                var temp = fromDate;
                fromDate = toDate;
                toDate = temp;
            }

            var username = this.GetUser()?.Username;
            var visits = await this.visitsRepository.ByUser(username, fromDate, toDate);
            return this.Success(visits.ToArray());
        }
    }
}
