namespace Clinic.Models.Authorization
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterModel : LoginModel
    {
        [RegularExpression(@"^[А-Яа-я\-\s]+$", ErrorMessage = "Только кириллица, дефис или пробел")]
        [MaxLength(50, ErrorMessage = "Не более 50 символов")]
        [MinLength(2, ErrorMessage = "Не менее 2 символов")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[А-Яа-я\-\s]+$", ErrorMessage = "Только кириллица, дефис или пробел")]
        [MaxLength(50, ErrorMessage = "Не более 50 символов")]
        [MinLength(2, ErrorMessage = "Не менее 2 символов")]
        public string SecondName { get; set; }

        [RegularExpression(@"^[А-Яа-я\-\s]+$", ErrorMessage = "Только кириллица, дефис или пробел")]
        [MaxLength(50, ErrorMessage = "Не более 50 символов")]
        [MinLength(2, ErrorMessage = "Не менее 2 символов")]
        public string ThirdName { get; set; }

        [Range(89000000000, 89999999999, ErrorMessage = "Формат 89XXXXXXXXX")]
        public long Phone { get; set; }
    }
}