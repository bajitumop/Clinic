namespace Clinic.Controllers
{
    using System.Data;
    using System.Threading.Tasks;
    using Services;
    using Dapper;
    using DataAccess.Repositories;
    using Microsoft.AspNetCore.Mvc;

    // ToDo: remove in production
    [ApiController]
    [Route("database")]
    public class DatabaseController : BaseController
    {
        private readonly DatabaseInitializer databaseInitializer;
        private readonly CryptoService cryptoService;
        private readonly IUsersRepository usersRepository;
        private readonly IDbConnection connection;

        public DatabaseController(DatabaseInitializer databaseInitializer, CryptoService cryptoService, IUsersRepository usersRepository, IDbConnection connection)
        {
            this.databaseInitializer = databaseInitializer;
            this.cryptoService = cryptoService;
            this.usersRepository = usersRepository;
            this.connection = connection;
        }
        
        [HttpPost, Route("init")]
        public async Task<IActionResult> InitDb()
        {
            await this.databaseInitializer.Init();
            return this.Success();
        }
        
        [HttpDelete, Route("drop-create")]
        public async Task<IActionResult> Truncate()
        {
            await this.connection.ExecuteAsync("truncate table users");
            await this.connection.ExecuteAsync("truncate table weekdays");
            await this.connection.ExecuteAsync("truncate table services");
            await this.connection.ExecuteAsync("truncate table schedules");
            await this.connection.ExecuteAsync("truncate table images");
            await this.connection.ExecuteAsync("truncate table visits");
            await this.connection.ExecuteAsync("delete from doctors");

            return await this.InitDb();
        }
    }
}
