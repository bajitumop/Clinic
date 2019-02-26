namespace Clinic.Controllers
{
    using System.Threading.Tasks;
    
    using Clinic.DataAccess.Repositories;

    using Microsoft.AspNetCore.Mvc;

    [Route("services")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly IServicesRepository servicesRepository;

        public ServiceController(IServicesRepository servicesRepository)
        {
            this.servicesRepository = servicesRepository;
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var service = await this.servicesRepository.GetAsync(id);
            if (service != null)
            {
                return this.Success(service);
            }

            return this.Error("Услуга не найдена");
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> All()
        {
            return this.Success(await this.servicesRepository.All());
        }
    }
}