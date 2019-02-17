namespace Clinic.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Doctor
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string ThirdName { get; set; }

        public string ImageUrl { get; set; }

        public string Info { get; set; }

        public string[] Positions { get; set; }

        public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; }

        public Dictionary<Specialty, Dictionary<DayOfWeek, TimeSpan[]>> Schedule { get; set; }
    }
}
