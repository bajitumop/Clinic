namespace Clinic.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Clinic.DataAccess;
    using Clinic.Services;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("test")]
    [ApiController]
    public class TestController : BaseController
    {
        private readonly CryptoService cryptoService;
        private readonly DataContext context;

        public TestController(CryptoService cryptoService, DataContext context)
        {
            this.cryptoService = cryptoService;
            this.context = context;
        }

        [HttpGet]
        [Route("encryptAccessToken")]
        public long EncryptToken(string accessToken)
        {
            return this.cryptoService.Decrypt<long>(accessToken);
        }
        
        [HttpGet, Route("addweekdays")]
        public async Task<IActionResult> AddWeekdays()
        {
            var schedules = await context.Schedules.ToListAsync();
            foreach (var schedule in schedules)
            {
                schedule.Weekdays = new[]
                                        {
                                            new DateTime(2019, 1, 1),
                                            new DateTime(2019, 2, 23),
                                            new DateTime(2019, 3, 8),
                                            new DateTime(2019, 5, 1),
                                            new DateTime(2019, 5, 9),
                                            new DateTime(2019, 6, 12),
                                            new DateTime(2019, 11, 4),
                                            new DateTime(2019, 12, 31)
                                        };
            }

            this.context.Schedules.UpdateRange(schedules);
            await this.context.SaveChangesAsync();
            return this.Success();
        }
    }
}
