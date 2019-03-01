namespace Clinic.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;

    public class DataRepository<T> : IRepository<T> where T : Entity
    {
        private readonly DataContext dataContext;

        public DataRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.Entities = dataContext.Set<T>();
        }

        protected DbSet<T> Entities { get; }

        public virtual async Task<T> GetAsync(long id)
        {
            return await this.dataContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<List<T>> All()
        {
            return await this.Entities.ToListAsync();
        }

        public virtual async Task DeleteAsync(long id)
        {
            var entity = await this.GetAsync(id);
            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            this.dataContext.Set<T>().Remove(entity);
        }

        public virtual void Update(T entity)
        {
            this.dataContext.Set<T>().Update(entity);
        }

        public virtual void Create(T entity)
        {
            this.dataContext.Set<T>().Add(entity);
        }

        public virtual async Task SaveChangesAsync()
        {
            await this.dataContext.SaveChangesAsync();
        }
    }
}
