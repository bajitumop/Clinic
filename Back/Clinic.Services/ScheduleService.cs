using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinic.Domain;
using Clinic.Models.Schedules;

namespace Clinic.Services
{
    public class ScheduleService
    {
        private static readonly TimeSpan VisitDuration = TimeSpan.FromMinutes(10);
        
        public IEnumerable<VisitStatusInfoModel> GetDoctorVisitsByDate(Schedule schedule, DateTime date, Visit[] visits)
        {
            TimeSpan? startTime = null;
            TimeSpan? endTime = null;

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    startTime = schedule.MondayStart;
                    endTime = schedule.MondayEnd;
                    break;
                case DayOfWeek.Tuesday:
                    startTime = schedule.TuesdayStart;
                    endTime = schedule.TuesdayEnd;
                    break;
                case DayOfWeek.Wednesday:
                    startTime = schedule.WednesdayStart;
                    endTime = schedule.WednesdayEnd;
                    break;
                case DayOfWeek.Thursday:
                    startTime = schedule.ThursdayStart;
                    endTime = schedule.ThursdayEnd;
                    break;
                case DayOfWeek.Friday:
                    startTime = schedule.FridayStart;
                    endTime = schedule.FridayEnd;
                    break;
                case DayOfWeek.Saturday:
                    startTime = schedule.SaturdayStart;
                    endTime = schedule.SaturdayEnd;
                    break;
            }

            if (!startTime.HasValue || !endTime.HasValue)
            {
                return Enumerable.Empty<VisitStatusInfoModel>();
            }
            
            var visitsCount = (int)((endTime.Value - startTime.Value).Ticks / VisitDuration.Ticks);
            return Enumerable
                .Range(0, visitsCount)
                .Select(i =>
                {
                    var dateTime = date.Add(startTime.Value).Add(TimeSpan.FromTicks(VisitDuration.Ticks * i));
                    var visit = visits.FirstOrDefault(v => v.DateTime == dateTime);

                    return new VisitStatusInfoModel
                    {
                        DateTime = dateTime,
                        Status = visit == null ? VisitStatus.Opened : VisitStatus.Closed
                    };
                });
        }
    }
}
