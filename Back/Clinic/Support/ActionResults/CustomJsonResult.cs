namespace Clinic.Support.ActionResults
{
    using System.Net;
    using System.Threading.Tasks;

    using Clinic.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    public class CustomJsonResult : StatusCodeResult
    {
        private static readonly JsonSerializerSettings Settings
            = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

        private readonly OperationResult result;
        
        public CustomJsonResult(OperationResult result, HttpStatusCode httpStatusCode)
            : base((int)httpStatusCode)
        {
            this.result = result;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            if (this.result != null)
            {
                var serialized = JsonConvert.SerializeObject(this.result, Settings);
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.Headers["Content-Encoding"] = "UTF-8";
                await context.HttpContext.Response.WriteAsync(serialized);
            }
        }

        public override void ExecuteResult(ActionContext context)
        {
            this.ExecuteResultAsync(context).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
