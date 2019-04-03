﻿namespace Clinic.Models.Schedules
{
    using System;

    using Doctors;

    public class ScheduleByWeekModel
    {
        public DoctorShortModel Doctor { get; set; }

        public string Specialty { get; set; }

        public TimeSpan? MondayStart { get; set; }

        public TimeSpan? MondayEnd { get; set; }

        public TimeSpan? TuesdayStart { get; set; }

        public TimeSpan? TuesdayEnd { get; set; }

        public TimeSpan? WednesdayStart { get; set; }

        public TimeSpan? WednesdayEnd { get; set; }

        public TimeSpan? FridayStart { get; set; }

        public TimeSpan? FridayEnd { get; set; }

        public TimeSpan? ThursdayStart { get; set; }

        public TimeSpan? ThursdayEnd { get; set; }

        public TimeSpan? SaturdayStart { get; set; }

        public TimeSpan? SaturdayEnd { get; set; }
    }
}