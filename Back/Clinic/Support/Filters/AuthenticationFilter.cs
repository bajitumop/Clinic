namespace Clinic.Support.Filters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;
    using Clinic.Models.OperationResults;
    using Clinic.Services;
    using Clinic.Support.ActionResults;

    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Net.Http.Headers;

    public class AuthenticationFilter : IAsyncActionFilter
    {
        private const string Bearer = "Bearer";

        private readonly IUsersRepository usersRepository;
        private readonly CryptoService cryptoService;

        public AuthenticationFilter(IUsersRepository usersRepository, CryptoService cryptoService)
        {
            this.usersRepository = usersRepository;
            this.cryptoService = cryptoService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
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
                var username = this.cryptoService.Decrypt<string>(token);
                var user = await this.usersRepository.GetAsync(username);
                if (user == null)
                {
                    var operationResult = new OperationResult(false, "Указанный токен доступа не прошел проверку");
                    context.Result = new CustomJsonResult(operationResult, HttpStatusCode.Unauthorized);
                    return;
                }

                context.HttpContext.Items[nameof(User)] = user;
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
