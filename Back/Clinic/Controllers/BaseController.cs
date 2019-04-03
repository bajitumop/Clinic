namespace Clinic.Controllers
{
    using System.Net;

    using Domain;
    using Models.OperationResults;
    using Support.ActionResults;

    using Microsoft.AspNetCore.Mvc;

    public abstract class BaseController : ControllerBase
    {
        protected IActionResult Success(HttpStatusCode statusCode = HttpStatusCode.OK)
            => new CustomJsonResult(new OperationResult(true), statusCode);

        protected IActionResult Success<T>(T data, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new CustomJsonResult(new ContentOperationResult<T>(true, data, message), statusCode);

        protected IActionResult Error(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new CustomJsonResult(new OperationResult(false, message), statusCode);

        protected IActionResult Error<T>(T data, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new CustomJsonResult(new ContentOperationResult<T>(false, data, message), statusCode);

        protected User GetUser() => (User)this.HttpContext.Items[nameof(Domain.User)];
    }
}
