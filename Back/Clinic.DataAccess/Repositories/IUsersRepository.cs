namespace Clinic.DataAccess.Repositories
{
    using System.Threading.Tasks;
    using Domain;

    public interface IUsersRepository
    {
        Task<bool> IsLastAdmin(string username);

        Task<User> GetAsync(string username);

        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(string username);
    }
}
