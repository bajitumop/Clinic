namespace Clinic.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Specialty
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
