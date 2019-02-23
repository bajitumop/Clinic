namespace Clinic.Middlewares
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using Clinic.Models.OperationResults;
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception exc)
            {
                this.logger.LogError(exc, exc.Message);
                var operationResult = new OperationResult(false, exc.ToString());
                var customJsonResult = new CustomJsonResult(operationResult, HttpStatusCode.InternalServerError);
                await customJsonResult.ExecuteResultAsync(new ActionContext { HttpContext = context });
            }
        }
    }
}
