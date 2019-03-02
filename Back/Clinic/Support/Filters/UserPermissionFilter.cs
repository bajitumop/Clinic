namespace Clinic.Support.Filters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;
    using Clinic.Models.OperationResults;
    using Clinic.Services;
    using Clinic.Support.ActionResults;

    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Net.Http.Headers;

    public class UserPermissionFilter : IAsyncActionFilter
    {
        private const string Bearer = "Bearer";

        private readonly IUsersRepository usersRepository;
        private readonly CryptoService cryptoService;

        public UserPermissionFilter(IUsersRepository usersRepository, CryptoService cryptoService)
        {
            this.usersRepository = usersRepository;
            this.cryptoService = cryptoService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Filters.Any(filter => filter is IAllowAnonymousFilter))
            {
                await next();
                return;
            }

            var requiredUserPermission = (context.Filters.FirstOrDefault(filter => filter is UserPermissionAttribute) as UserPermissionAttribute)?.UserPermission;
            if (!requiredUserPermission.HasValue)
            {
                await next();
                return;
            }

            var authorizationHeader = context.HttpContext.Request.Headers[HeaderNames.Authorization].FirstOrDefault()?.Trim();
            if (authorizationHeader == null)
            {
                await next();
                return;
            }

            if (!authorizationHeader.StartsWith(Bearer, true, CultureInfo.InvariantCulture))
            {
                var operationResult = new OperationResult(false, "Поддерживается только Bearer схема авторизации");
                context.Result = new CustomJsonResult(operationResult, HttpStatusCode.Unauthorized);
                return;
            }

            try
            {
                var token = authorizationHeader.Substring(Bearer.Length).Trim();
                var userId = this.cryptoService.Decrypt<long>(token);
                var user = await this.usersRepository.GetAsync(userId);
                if (user == null)
                {
                    var operationResult = new OperationResult(false, "Указанный токен доступа не прошел проверку");
                    context.Result = new CustomJsonResult(operationResult, HttpStatusCode.Unauthorized);
                    return;
                }
                
                if ((requiredUserPermission & user.Permission) != requiredUserPermission)
                {
                    var operationResult = new OperationResult(false, "Не хватает прав для совершения этой операции");
                    context.Result = new CustomJsonResult(operationResult, HttpStatusCode.Forbidden);
                    return;
                }

                await next();
            }
            catch (Exception)
            {
                var operationResult = new OperationResult(false, "Указанный токен доступа не прошел проверку");
                context.Result = new CustomJsonResult(operationResult, HttpStatusCode.Unauthorized);
            }
        }
    }
}
