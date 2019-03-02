namespace Clinic.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Clinic.Domain;

    public interface IServicesRepository : IRepository<Service>
    {
        Task<IEnumerable<Service>> GetBySpecialtyAsync(long specialtyId);

        Task<Service> GetAsync(long id);
    }
}
