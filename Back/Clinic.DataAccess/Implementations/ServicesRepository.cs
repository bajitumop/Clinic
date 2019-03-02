namespace Clinic.DataAccess.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;

    public class ServicesRepository : DataRepository<Service>, IServicesRepository
    {
        public ServicesRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<IEnumerable<Service>> GetBySpecialtyAsync(long specialtyId)
        {
            return await this.Entities.Where(d => d.Specialty.Id == specialtyId).Include(s => s.Specialty).ToArrayAsync();
        }

        public async Task<Service> GetAsync(long id)
        {
            return await this.Entities.Include(s => s.Specialty).FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<List<Service>> All()
        {
            return await this.Entities.Include(s => s.Specialty).ToListAsync();
        }
    }
}
