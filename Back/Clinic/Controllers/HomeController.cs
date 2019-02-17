namespace Clinic.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
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

        [HttpGet, Route("services")]
        public async Task<IEnumerable<Service>> Services()
        {
            return await this.dataContext.Services.ToListAsync();
        }

        [HttpGet, Route("specialties")]
        public async Task<IEnumerable<Specialty>> Specialties()
        {
            return await this.dataContext.Specialties.ToListAsync();
        }

        [HttpGet, Route("schedules")]
        public async Task<IEnumerable<Schedule>> Schedules()
        {
            return await this.dataContext.Schedules.ToListAsync();
        }
    }
}