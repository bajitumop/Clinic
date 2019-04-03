namespace Clinic.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Domain;

    public interface IDoctorsRepository
    {
        Task<IList<Doctor>> GetBySpecialtyAsync(string specialtyId);

        Task<IList<Doctor>> All();

        Task<Doctor> GetAsync(long id);

        Task CreateAsync(Doctor doctor);

        Task Delete(long id);
    }
}
