namespace Clinic.Models
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

        public List<Speciality> Specialities { get; set; }

        public Dictionary<Speciality, Dictionary<DayOfWeek, TimeSpan[]>> Schedule { get; set; }
    }
}
