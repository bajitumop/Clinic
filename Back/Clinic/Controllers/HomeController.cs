namespace Clinic.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.DataAccess;
    using Clinic.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("Home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DataContext dataContext;

        public HomeController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        
        [HttpGet, Route("Users")]
        public async Task<IEnumerable<User>> Users()
        {
            return await this.dataContext.Users.ToListAsync();
        }
    }
}