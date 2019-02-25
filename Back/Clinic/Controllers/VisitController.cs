namespace Clinic.Controllers
{
    using Clinic.Domain;
    using Clinic.Support.Filters;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("visits")]
    [UserPermission(UserPermission.CanVisitDoctor)]
    public class VisitController : BaseController
    {
    }
}
