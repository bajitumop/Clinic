namespace Clinic.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class Image : Entity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Format { get; set; }

        [Required]
        public byte[] Content { get; set; }
    }
}
