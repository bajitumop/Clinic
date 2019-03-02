namespace Clinic.Controllers
{
    using System.Threading.Tasks;
    using System.Web;

    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;
    using Clinic.Models.Authorization;
    using Clinic.Services;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [AllowAnonymous]
    [Route("account")]
    public class AccountController : BaseController
    {
        private readonly IUsersRepository usersRepository;
        private readonly CryptoService cryptoService;
        
        public AccountController(IUsersRepository usersRepository, CryptoService cryptoService)
        {
            this.usersRepository = usersRepository;
            this.cryptoService = cryptoService;
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            var user = await this.usersRepository.GetByUserName(registerModel.UserName);
            if (user != null)
            {
                return this.Error("Имя пользователя занято");
            }

            user = new User
                       {
                           Username = registerModel.UserName,
                           PasswordHash = registerModel.PasswordHash,
                           FirstName = registerModel.FirstName,
                           SecondName = registerModel.SecondName,
                           ThirdName = registerModel.ThirdName,
                           Phone = registerModel.Phone,
                           Permission = UserPermission.CanVisitDoctor,
                       };

            this.usersRepository.Create(user);
            await this.usersRepository.SaveChangesAsync();

            return this.Success(this.GenerateAccessToken(user.Username));
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await this.usersRepository.GetByUserName(loginModel.UserName);
            if (user == null)
            {
                return this.Error("Пользователь с таким именем не существует в базе");
            }

            if (user.PasswordHash != loginModel.PasswordHash)
            {
                return this.Error("Указано неверное имя пользователя или пароль");
            }

            return this.Success(this.GenerateAccessToken(user.Username));
        }
        
        private string GenerateAccessToken(string username)
        {
            return HttpUtility.UrlEncode(this.cryptoService.Encrypt(username));
        }
    }
}
