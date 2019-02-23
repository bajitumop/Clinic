namespace Clinic.Support.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Clinic.Models.OperationResults;
    using Clinic.Support.ActionResults;

    using Microsoft.AspNetCore.Mvc.Filters;

    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var dictionary = new Dictionary<string, string[]>();
                foreach (var modelState in context.ModelState)
                {
                    var errors = modelState.Value.Errors.Select(x => x.ErrorMessage).ToArray();
                    if (errors.Any())
                    {
                        dictionary[modelState.Key] = errors;
                    }
                }

                var operationResult = new ContentOperationResult<Dictionary<string, string[]>>(
                    false, 
                    dictionary, 
                    "Входные значения не прошли валидацию");

                context.Result = new CustomJsonResult(operationResult, HttpStatusCode.BadRequest);
            }
        }
    }
}
