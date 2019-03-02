namespace Clinic.Controllers
{
    using Clinic.Domain;
    using Clinic.Support.Filters;

    using Microsoft.AspNetCore.Mvc;

    [UserPermission(UserPermission.All)]
    [Route("users")]
    [ApiController]
    public class UserController : BaseController
    {
    }
}
