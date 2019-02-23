namespace Clinic.Controllers
{
    using System.Net;

    using Clinic.Models.OperationResults;
    using Clinic.Support.ActionResults;

    using Microsoft.AspNetCore.Mvc;

    public class BaseController : ControllerBase
    {
        protected IActionResult Success(HttpStatusCode statusCode = HttpStatusCode.NoContent)
            => new CustomJsonResult(new OperationResult(true), statusCode);

        protected IActionResult Success<T>(T data, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new CustomJsonResult(new ContentOperationResult<T>(true, data, message), statusCode);

        protected IActionResult Error(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new CustomJsonResult(new OperationResult(false, message), statusCode);

        protected IActionResult Error<T>(T data, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new CustomJsonResult(new ContentOperationResult<T>(false, data, message), statusCode);
    }
}
