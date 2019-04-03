namespace Clinic.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;
    using Clinic.Models.Users;
    using Clinic.Support.Filters;

    using Microsoft.AspNetCore.Mvc;

    [Route("users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUsersRepository usersRepository;
        private readonly IMapper mapper;

        public UserController(IUsersRepository usersRepository, IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.mapper = mapper;
        }

        [MustBeAuthorized]
        [HttpPut, Route("update")]
        public async Task<IActionResult> Update(UserEditModel model)
        {
            var authorizedUser = (User)this.HttpContext.Items[nameof(Domain.User)];
            if (authorizedUser.Username != model.Username)
            {
                return this.Error("Пользователю разрешено обновлять только свои данные");
            }

            var user = this.mapper.Map(model, authorizedUser);
            user.UserPermission = authorizedUser.UserPermission;

            await this.usersRepository.UpdateAsync(user);
            return this.Success();
        }
    }
}
