namespace Clinic.Controllers
{
    using System.Threading.Tasks;

    using Clinic.DataAccess;
    using Clinic.Domain;
    using Clinic.Services;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    // Uncomment when production db was created
    //[UserPermission(UserPermission.All)]
    [ApiController]
    [Route("database")]
    public class DatabaseController : BaseController
    {
        private readonly DataContext dataContext;
        private readonly DatabaseInitializer databaseInitializer;
        private readonly CryptoService cryptoService;

        public DatabaseController(DataContext dataContext, DatabaseInitializer databaseInitializer, CryptoService cryptoService)
        {
            this.dataContext = dataContext;
            this.databaseInitializer = databaseInitializer;
            this.cryptoService = cryptoService;
        }

        [HttpPost, Route("migrate")]
        public async Task<IActionResult> MigrateDb()
        {
            await this.dataContext.Database.MigrateAsync();
            return this.Success();
        }

        [HttpPost, Route("init")]
        public async Task<IActionResult> InitDb()
        {
            await this.databaseInitializer.Init();
            return this.Success();
        }

        [HttpPost, Route("createAdmin")]
        public IActionResult CreateAdmin()
        {
            var user = new User
            {
                UserName = "admin",
                FirstName = "Администратор",
                SecondName = "Администратор",
                ThirdName = "Администратор",
                Permissions = new[] { UserPermission.All },
                Phone = 89999999999,
                PasswordHash = "admin",
            };

            this.dataContext.Users.Add(user);
            this.dataContext.SaveChanges();

            var token = cryptoService.Encrypt(user.Id);
            user.PasswordHash = token;
            this.dataContext.SaveChanges();

            return this.Success();
        }
    }
}
