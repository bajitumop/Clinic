namespace Clinic.DataAccess.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;

    public class DoctorsRepository : DataRepository<Doctor>, IDoctorsRepository
    {
        public DoctorsRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialtyAsync(long specialtyId)
        {
            return await this.Entities.Where(d => d.Schedules.Any(s => s.SpecialtyId == specialtyId)).ToArrayAsync();
        }
    }
}
