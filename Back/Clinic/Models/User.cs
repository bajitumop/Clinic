namespace Clinic.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
