namespace Clinic.Support.ActionResults
{
    using System.Net;
    using System.Threading.Tasks;

    using Clinic.Models.OperationResults;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    public class CustomJsonResult : StatusCodeResult
    {
        private readonly OperationResult result;
        
        public CustomJsonResult(OperationResult result, HttpStatusCode httpStatusCode)
            : base((int)httpStatusCode)
        {
            this.result = result;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var serialized = JsonConvert.SerializeObject(this.result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Headers["Content-Encoding"] = "UTF-8";
            context.HttpContext.Response.StatusCode = this.StatusCode;
            await context.HttpContext.Response.WriteAsync(serialized);
        }
    }
}
