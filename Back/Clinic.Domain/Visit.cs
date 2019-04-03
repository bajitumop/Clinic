namespace Clinic.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Visit
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        public long DoctorId { get; set; }
        
        [Required]
        public long ServiceId { get; set; }
        
        [Required]
        public DateTime DateTime { get; set; }

        public VisitStatus Status { get; set; }

        public string RoomNumber { get; set; }
    }
}
