namespace Clinic.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.DataAccess;
    using Clinic.Domain;

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
        
        [HttpGet, Route("doctors")]
        public async Task<IEnumerable<Doctor>> Users()
        {
            return await this.dataContext.Doctors.ToListAsync();
        }
    }
}