namespace Clinic.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class User : Entity
    {
        [Key]
        public long Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }
        
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public long Phone { get; set; }
        
        public UserPermission[] Permissions { get; set; }
    }
}
