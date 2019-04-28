namespace Clinic.Mappings
{
    using AutoMapper;

    using Clinic.Domain;
    using Clinic.Models.Authorization;
    using Clinic.Models.Users;

    public class UserMapper : Profile
    {
        public UserMapper()
        {
            this.CreateMap<UserEditModel, User>();
            this.CreateMap<RegisterModel, User>();
            this.CreateMap<User, LoginResponse>();
        }
    }
}
