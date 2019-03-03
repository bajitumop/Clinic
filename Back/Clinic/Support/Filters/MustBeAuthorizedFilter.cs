namespace Clinic.Support.Filters
{
    using System.Linq;
    using System.Net;

    using Clinic.Domain;
    using Clinic.Models.OperationResults;
    using Clinic.Support.ActionResults;

    using Microsoft.AspNetCore.Mvc.Filters;

    public class MustBeAuthorizedFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.Filters.Any(x => x is MustBeAuthorizedAttribute)
                || (context.HttpContext.Items.TryGetValue(nameof(User), out var userObj) && userObj is User))
            {
                return;
            }

            var operationResult = new OperationResult(false, "Пользователь должен быть авторизован");
            context.Result = new CustomJsonResult(operationResult, HttpStatusCode.Forbidden);
        }
    }
}
