namespace Clinic.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Domain;

    public interface ISchedulesRepository
    {
        Task<Schedule> GetAsync(long doctorId, string specialty);

        Task UpsertAsync(Schedule schedule);

        Task Delete(long doctorId, string specialty);

        Task<List<Schedule>> All();

        Task<List<Schedule>> GetByDoctorAsync(long doctorId);
    }
}
