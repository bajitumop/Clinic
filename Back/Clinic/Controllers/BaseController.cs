namespace Clinic.Controllers
{
    using System.Net;

    using Clinic.Models;
    using Clinic.Support.ActionResults;

    using Microsoft.AspNetCore.Mvc;

    public class BaseController : ControllerBase
    {
        protected IActionResult Success<T>(T data)
        {
            return new CustomJsonResult(new SuccessOperationResult<T>(data), HttpStatusCode.OK);
        }

        protected IActionResult Success() => new CustomJsonResult(null, HttpStatusCode.NoContent);

        protected IActionResult Error(string message)
        {
            return new CustomJsonResult(new ErrorOperationResult(message), HttpStatusCode.BadRequest);
        }
    }
}
