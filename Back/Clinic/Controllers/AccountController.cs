namespace Clinic.Controllers
{
    using System.Threading.Tasks;
    using System.Web;

    using AutoMapper;

    using DataAccess.Repositories;
    using Domain;
    using Models.Authorization;
    using Services;

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
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (await this.usersRepository.GetAsync(registerModel.Username) != null)
            {
                return this.Error("Имя пользователя занято");
            }

            var user = this.mapper.Map<User>(registerModel);
            user.UserPermission = UserPermission.CanVisitDoctor;

            await this.usersRepository.CreateAsync(user);

            return this.Success(this.GenerateAccessToken(user.Username));
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
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

            var response = this.mapper.Map<LoginResponse>(user);
            response.AccessToken = this.GenerateAccessToken(user.Username);
            return this.Success(response);
        }
        
        private string GenerateAccessToken(string username)
        {
            return HttpUtility.UrlEncode(this.cryptoService.Encrypt(username));
        }
    }
}
