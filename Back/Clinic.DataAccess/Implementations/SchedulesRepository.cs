namespace Clinic.DataAccess.Implementations
{
    using System.Threading.Tasks;

    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;

    public class SchedulesRepository : DataRepository<Schedule>, ISchedulesRepository
    {
        public SchedulesRepository(DataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<Schedule> GetAsync(long doctorId, long specialtyId)
        {
            return await this.Entities.Include(s => s.Doctor).Include(s => s.Specialty).FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.SpecialtyId == specialtyId);
        }
    }
}
