namespace Clinic.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        [Required]
        public long Phone { get; set; }
        
        public UserPermission UserPermission { get; set; }

        public bool IsAdmin => (UserPermission & UserPermission.All) == UserPermission.All;
    }
}
