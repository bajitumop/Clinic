namespace Clinic.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Domain;

    public interface IDoctorsRepository : IRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetBySpecialtyAsync(long specialtyId);
    }
}
