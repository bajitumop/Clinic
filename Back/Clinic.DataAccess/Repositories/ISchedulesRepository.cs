namespace Clinic.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Domain;

    public interface ISchedulesRepository
    {
        Task<Schedule> GetAsync(long doctorId);

        Task UpsertAsync(Schedule schedule);

        Task Delete(long doctorId);

        Task<IEnumerable<Schedule>> All();
    }
}
