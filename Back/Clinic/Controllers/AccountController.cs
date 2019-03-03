namespace Clinic.Controllers
{
    using System.Threading.Tasks;
    using System.Web;

    using AutoMapper;

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
        private readonly IMapper mapper;
        
        public AccountController(IUsersRepository usersRepository, CryptoService cryptoService, IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.cryptoService = cryptoService;
            this.mapper = mapper;
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (await this.usersRepository.GetAsync(registerModel.Username) != null)
            {
                return this.Error("Имя пользователя занято");
            }

            var user = this.mapper.Map<User>(registerModel);
            user.Permission = UserPermission.CanVisitDoctor;

            this.usersRepository.Create(user);
            await this.usersRepository.SaveChangesAsync();

            return this.Success(this.GenerateAccessToken(user.Username));
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = await this.usersRepository.GetAsync(loginModel.Username);
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
