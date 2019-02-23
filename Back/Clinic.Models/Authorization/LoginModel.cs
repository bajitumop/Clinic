namespace Clinic.Models.Authorization
{
    using System.ComponentModel.DataAnnotations;

    public class LoginModel
    {
        [RegularExpression(@"^[A-Za-z0-9_\-\.]+$", ErrorMessage = "Только латиница, цифры, точка, дефис или нижнее подчеркивание")]
        [Required(ErrorMessage = "Имя пользователя не может быть пустым")]
        public string UserName { get; set; }

        [MaxLength(64, ErrorMessage = "Хеш пароля должен содержать 64 символа")]
        [MinLength(64, ErrorMessage = "Хеш пароля должен содержать 64 символа")]
        [Required(ErrorMessage = "Хеш пароля не может быть пустым")]
        public string PasswordHash { get; set; }
    }
}
