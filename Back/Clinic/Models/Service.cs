namespace Clinic.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Service
    {
        [Required]
        public Specialty Specialty { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
