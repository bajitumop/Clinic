namespace Clinic.Mappings
{
    using AutoMapper;

    using Clinic.Domain;
    using Clinic.Models.Schedules;

    public class ScheduleMapper : Profile
    {
        public ScheduleMapper()
        {
            this.CreateMap<Schedule, ScheduleByWeekModel>();
        }
    }
}
