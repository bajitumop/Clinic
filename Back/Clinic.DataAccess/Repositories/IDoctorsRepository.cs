namespace Clinic.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Domain;

    public interface IDoctorsRepository
    {
        Task<IEnumerable<Doctor>> GetBySpecialtyAsync(string specialtyId);

        Task<IEnumerable<Doctor>> All();

        Task<Doctor> GetAsync(long id);

        Task CreateAsync(Doctor doctor);
    }
}
