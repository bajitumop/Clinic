namespace Clinic.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Specialty
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<DoctorSpecialty> DoctorSpecialties { get; set; }
        
        public ICollection<Service> Services { get; set; }
    }
}
