namespace Clinic.Support.Filters
{
    using Clinic.Domain;

    using Microsoft.AspNetCore.Mvc.Filters;

    public class UserPermissionAttribute : ActionFilterAttribute
    {
        public UserPermissionAttribute(UserPermission userPermission)
        {
            this.UserPermission = userPermission;
        }

        public UserPermission UserPermission { get; }
    }
}
