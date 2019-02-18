namespace Clinic.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Domain;

    public interface IRepository<T> where T : Entity
    {
        Task<T> GetAsync(long id);

        Task<List<T>> All();

        Task DeleteAsync(T entity);

        Task DeleteAsync(long id);

        Task UpdateAsync(T entity);

        Task CreateAsync(T entity);

        Task SaveChangesAsync();
    }
}
