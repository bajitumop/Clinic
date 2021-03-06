﻿namespace Clinic.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Domain;

    public interface IServicesRepository
    {
        Task<IEnumerable<Service>> GetBySpecialtyAsync(string specialty);

        Task<Service> GetAsync(long id);

        Task<IEnumerable<Service>> All();

        Task CreateAsync(Service service);

        Task UpdateAsync(Service service);

        Task DeleteAsync(long id);
    }
}
