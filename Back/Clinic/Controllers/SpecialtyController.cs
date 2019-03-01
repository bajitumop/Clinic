namespace Clinic.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Clinic.DataAccess.Repositories;
    using Clinic.Models;

    using Microsoft.AspNetCore.Mvc;

    [Route("specialties")]
    [ApiController]
    public class SpecialtyController : BaseController
    {
        private readonly ISpecialtiesRepository specialtiesRepository;
        private readonly IMapper mapper;

        public SpecialtyController(ISpecialtiesRepository specialtiesRepository, IMapper mapper)
        {
            this.specialtiesRepository = specialtiesRepository;
            this.mapper = mapper;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> GetAll()
        {
            var specialties = await this.specialtiesRepository.All();
            return this.Success(specialties.Select(this.mapper.Map<SpecialtyModel>).ToArray());
        }

        [HttpGet, Route("get")]
        public async Task<IActionResult> Get(long id)
        {
            var specialty = await this.specialtiesRepository.GetAsync(id);
            return this.Success(this.mapper.Map<SpecialtyModel>(specialty));
        }
    }
}
