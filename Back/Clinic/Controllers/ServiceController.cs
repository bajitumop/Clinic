namespace Clinic.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Clinic.DataAccess.Repositories;
    using Clinic.Models;

    using Microsoft.AspNetCore.Mvc;

    [Route("services")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly IServicesRepository servicesRepository;
        private readonly IMapper mapper;

        public ServiceController(IServicesRepository servicesRepository, IMapper mapper)
        {
            this.servicesRepository = servicesRepository;
            this.mapper = mapper;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> All()
        {
            var services = await this.servicesRepository.All();
            return this.Success(services.Select(this.mapper.Map<ServiceModel>).ToArray());
        }

        [HttpGet, Route("get")]
        public async Task<IActionResult> Get(long id)
        {
            var service = await this.servicesRepository.GetAsync(id);
            if (service == null)
            {
                return this.Error("Услуга не найдена");
            }

            return this.Success(this.mapper.Map<ServiceModel>(service));
        }

        [HttpGet, Route("by-specialty")]
        public async Task<IActionResult> GetBySpecialty(long specialtyId)
        {
            var services = await this.servicesRepository.GetBySpecialtyAsync(specialtyId);
            return this.Success(services.Select(this.mapper.Map<ServiceModel>).ToArray());
        }
    }
}