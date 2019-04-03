﻿namespace Clinic.DataAccess.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Repositories;
    using Domain;
    
    public class ServicesRepository : IServicesRepository
    {
        private readonly IDbConnection connection;

        public ServicesRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IList<Service>> GetBySpecialtyAsync(string specialty)
        {
            return (await this.connection.QueryAsync<Service>(@"select * from services where ""Specialty"" = @specialty", new { specialty })).ToList();
        }

        public async Task<Service> GetAsync(long id)
        {
            return await this.connection.QueryFirstOrDefaultAsync<Service>(@"select * from services where ""Id"" = @id", new {id});
        }

        public async Task<IList<Service>> All()
        {
            return (await this.connection.QueryAsync<Service>(@"select * from services")).ToList();
        }

        public async Task CreateAsync(Service service)
        {
            var id = await this.connection.ExecuteAsync(@"
                insert into services (""Price"", ""AdditionalInfo"", ""Description"", ""Specialty"", ""DoctorPermission"")
                    values (@Price, @AdditionalInfo, @Description, @Specialty, @DoctorPermission)
                    returning ""Id""",
                service);
            service.Id = id;
        }

        public async Task UpdateAsync(Service service)
        {
            await this.connection.ExecuteAsync(@"
                update services set 
                    ""Price"" = @Price, 
                    ""AdditionalInfo"" = @AdditionalInfo, 
                    ""Description"" = @Description, 
                    ""Specialty"" = @Specialty, 
                    ""DoctorPermission"" = @DoctorPermission
                    where ""Id"" = @Id",
                service);
        }

        public async Task DeleteAsync(long id)
        {
            await this.connection.ExecuteAsync(@"delete from services where ""Id"" = @id", new {id});
        }
    }
}
