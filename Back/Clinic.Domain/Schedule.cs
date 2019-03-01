namespace Clinic.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Schedule : Entity
    {
        [Required]
        public long DoctorId { get; set; }

        [Required]
        public Doctor Doctor { get; set; }

        [Required]
        public long SpecialtyId { get; set; }

        [Required]
        public Specialty Specialty { get; set; }

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

        public TimeSpan VisitDuration { get; set; }

        public ICollection<DateTime> Weekdays { get; set; }
    }
}
