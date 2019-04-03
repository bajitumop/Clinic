namespace Clinic.Domain
{
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
        
        public string Info { get; set; }

        public string[] Positions { get; set; }
        
        public long? ImageId { get; set; }

        public DoctorPermission DoctorPermission { get; set; }
    }
}
