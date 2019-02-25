namespace Clinic.Controllers
{
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;
    using Clinic.Services;

    using Microsoft.AspNetCore.Mvc;

    [Route("test")]
    [ApiController]
    public class TestController : BaseController
    {
        private readonly CryptoService cryptoService;
        private readonly IUsersRepository usersRepository;

        public TestController(CryptoService cryptoService, IUsersRepository usersRepository)
        {
            this.cryptoService = cryptoService;
            this.usersRepository = usersRepository;
        }

        [HttpGet]
        [Route("encryptAccessToken")]
        public long EncryptToken(string accessToken)
        {
            return this.cryptoService.Decrypt<long>(accessToken);
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> Users()
        {
            return this.Success(await this.usersRepository.All());
        }
    }
}
