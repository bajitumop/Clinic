namespace Clinic.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface IVisitsRepository
    {
        Task<IList<Visit>> ByUser(string username, DateTime from, DateTime to);

        Task<IList<Visit>> All(DateTime from, DateTime to);

        Task Create(string username, long serviceId, long doctorId, DateTime dateTime);

        Task Delete(string username, long id);
    }
}
