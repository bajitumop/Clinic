namespace Clinic.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class User : Entity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        [Required]
        public long Phone { get; set; }
        
        public UserPermission[] Permissions { get; set; }
    }
}
