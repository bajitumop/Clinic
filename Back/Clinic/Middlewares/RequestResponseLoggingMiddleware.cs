namespace Clinic.Middlewares
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            this.logger.LogInformation($"Start request: {request.Path}{request.QueryString}");
            await this.next(context);
            this.logger.LogInformation($"End request: {request.Path}{request.QueryString}");
        }
    }
}
