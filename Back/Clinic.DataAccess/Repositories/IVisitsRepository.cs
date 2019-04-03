namespace Clinic.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface IVisitsRepository
    {
        Task<IList<Visit>> FromRange(string username, DateTime from, DateTime to);
    }
}
