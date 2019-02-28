namespace Clinic.Controllers
{
    using System.Net;
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("images")]
    public class ImagesController : BaseController
    {
        private readonly IImagesRepository imagesRepository;

        public ImagesController(IImagesRepository imagesRepository)
        {
            this.imagesRepository = imagesRepository;
        }

        [HttpGet, Route("get")]
        public async Task<IActionResult> Get(long id)
        {
            var image = await this.imagesRepository.GetAsync(id);
            if (image == null)
            {
                return this.StatusCode((int)HttpStatusCode.NotFound);
            }

            return this.File(image.Content, image.Format);
        }
    }
}
