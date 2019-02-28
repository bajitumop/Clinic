namespace Clinic.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Doctor : Entity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string ThirdName { get; set; }

        [ForeignKey(nameof(ImageId))]
        public Image Image { get; set; }

        public long? ImageId { get; set; }

        public string Info { get; set; }

        public string[] Positions { get; set; }

        public ICollection<Schedule> Schedules { get; set; }

        public ICollection<Visit> Visits { get; set; }

        public int Room { get; set; }
    }
}
