namespace Clinic.Middlewares
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Clinic.Models;
    using Clinic.Support.ActionResults;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception exc)
            {
                this.logger.LogError(exc, exc.Message);
                var operationResult = new ErrorOperationResult(exc.ToString());
                var customJsonResult = new CustomJsonResult(operationResult, HttpStatusCode.InternalServerError);
                customJsonResult.ExecuteResult(new ActionContext { HttpContext = context });
            }
        }
    }
}
