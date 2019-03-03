namespace Clinic.Support.Filters
{
    using System.Linq;
    using System.Net;

    using Clinic.Domain;
    using Clinic.Models.OperationResults;
    using Clinic.Support.ActionResults;

    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    
    public class AuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Filters.Any(filter => filter is IAllowAnonymousFilter))
            {
                return;
            }

            var requiredUserPermission = (context.Filters.FirstOrDefault(filter => filter is UserPermissionAttribute) as UserPermissionAttribute)?.UserPermission;
            if (!requiredUserPermission.HasValue)
            {
                return;
            }

            if (!context.HttpContext.Items.TryGetValue(nameof(User), out var userObj) || !(userObj is User user))
            {
                var operationResult = new OperationResult(false, "Пользователь должен быть авторизован в системе");
                context.Result = new CustomJsonResult(operationResult, HttpStatusCode.Forbidden);
                return;
            }

            if ((requiredUserPermission & user.Permission) != requiredUserPermission)
            {
                var operationResult = new OperationResult(false, "Не хватает прав для выполнения данной операции");
                context.Result = new CustomJsonResult(operationResult, HttpStatusCode.Forbidden);
            }
        }
    }
}
