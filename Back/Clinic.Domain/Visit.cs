namespace Clinic.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Visit : Entity
    {
        [Key]
        public long Id { get; set; }

        public long DoctorId { get; set; }

        public Doctor Doctor { get; set; }

        public long ServiceId { get; set; }

        public Service Service { get; set; }

        public DateTime DateTime { get; set; }
    }
}
