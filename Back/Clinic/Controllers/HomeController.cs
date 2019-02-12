namespace Clinic.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.DataAccess;
    using Clinic.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    [Route("Home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly ILogger logger;

        public HomeController(DataContext dataContext, ILogger<HomeController> logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
        }
        
        [HttpGet, Route("Users")]
        public async Task<IEnumerable<User>> Users(int x = 5, int z = 7)
        {
            return await this.dataContext.Users.ToListAsync();
        }
    }
}