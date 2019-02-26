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
        
        public async Task<User> GetByUserName(string userName)
        {
            return await this.Entities.FirstOrDefaultAsync(user => user.UserName == userName);
        }
    }
}
