namespace Clinic.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Clinic.DataAccess.Repositories;

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
                return Success(service);
            }
            else
            {
                return Error("Услуга не найдена");
            }
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> All()
        {
            return Success(await servicesRepository.All());
        }
    }
}