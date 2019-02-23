namespace Clinic.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Domain;

    public interface IRepository<T> where T : Entity
    {
        Task<T> GetAsync(long id);

        Task<List<T>> All();

        void Delete(T entity);

        Task DeleteAsync(long id);

        void Update(T entity);

        void Create(T entity);

        Task SaveChangesAsync();
    }
}
