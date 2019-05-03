namespace Clinic.DataAccess.Repositories
{
    using System.Threading.Tasks;
    using Domain;

    public interface IUsersRepository
    {
        Task<User> GetAsync(string username);

        Task CreateAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(string username);
    }
}
