namespace Clinic.DataAccess.Repositories
{
    using System.Threading.Tasks;

    using Clinic.Domain;

    public interface ISchedulesRepository : IRepository<Schedule>
    {
        Task<Schedule> GetAsync(long doctorId, long specialtyId);
    }
}
