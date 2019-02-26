namespace Clinic.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Visit : Entity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long DoctorId { get; set; }

        [Required]
        public Doctor Doctor { get; set; }

        [Required]
        public long ServiceId { get; set; }

        [Required]
        public Service Service { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}
