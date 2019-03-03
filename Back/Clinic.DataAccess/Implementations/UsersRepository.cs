namespace Clinic.DataAccess.Implementations
{
    using System.Linq;
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;

    public class UsersRepository : DataRepository<User>, IUsersRepository
    {
        public UsersRepository(DataContext dataContext)
            : base(dataContext)
        {
        }
        
        public async Task<bool> IsLastAdmin(string username)
        {
            return !(await this.Entities.AnyAsync(user => user.Permission == UserPermission.All && user.Username != username));
        }
    }
}
