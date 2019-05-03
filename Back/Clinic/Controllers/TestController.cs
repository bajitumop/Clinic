namespace Clinic.Controllers
{
    using Services;

    using Microsoft.AspNetCore.Mvc;

    [Route("test")]
    [ApiController]
    public class TestController : BaseController
    {
        private readonly CryptoService cryptoService;

        public TestController(CryptoService cryptoService)
        {
            this.cryptoService = cryptoService;
        }

        [HttpGet]
        [Route("encryptAccessToken")]
        public long EncryptToken(string accessToken)
        {
            return this.cryptoService.Decrypt<long>(accessToken);
        }

        [HttpGet, Route("fallback")]
        public string Fallback()
        {
            return "This is the default fallback answer";
        }
    }
}
