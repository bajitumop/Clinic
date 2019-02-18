namespace Clinic.DataAccess
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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

        protected IQueryable<T> Entities { get; }

        public async Task<T> GetAsync(long id)
        {
            return await this.dataContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> All()
        {
            return await this.Entities.ToListAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await this.GetAsync(id);
            if (entity != null)
            {
                await this.DeleteAsync(entity);
            }
        }

        [SuppressMessage("Compiler", "CS1998")]
        public async Task DeleteAsync(T entity)
        {
            this.dataContext.Set<T>().Remove(entity);
        }

        [SuppressMessage("Compiler", "CS1998")]
        public async Task UpdateAsync(T entity)
        {
            this.dataContext.Set<T>().Update(entity);
        }

        [SuppressMessage("Compiler", "CS1998")]
        public async Task CreateAsync(T entity)
        {
            this.dataContext.Set<T>().Add(entity);
        }

        public async Task SaveChangesAsync()
        {
            await this.dataContext.SaveChangesAsync();
        }
    }
}
