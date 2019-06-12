using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Domain;
using Clinic.Models.Schedules;
using Clinic.Services;

namespace Clinic.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("schedules")]
    public class ScheduleController : BaseController
    {
        private readonly ScheduleService scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetDoctorSchedule(int doctorId)
        {
            var records = await this.scheduleService.GetDoctorVisits(doctorId);
            PopulateRandomRecords(records);
            return Success(records);
        }

        private void PopulateRandomRecords(IList<VisitStatusInfoModel> records)
        {
            var random = new Random();
            foreach (var statusInfo in records)
            {
                if (random.Next() % 2 == 0)
                {
                    //statusInfo.Status = VisitStatus.Closed;
                }
            }
        }

    }
}
