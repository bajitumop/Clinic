namespace Clinic.DataAccess.Repositories
{
    using System.Threading.Tasks;

    using Clinic.Domain;

    public interface IUsersRepository : IRepository<User>
    {
        Task<bool> CheckLoginUnique(string login);

        Task<User> GetByUserName(string userName);
    }
}
