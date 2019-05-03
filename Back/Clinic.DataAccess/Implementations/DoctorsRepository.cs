namespace Clinic.DataAccess.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Repositories;
    using Domain;
    
    public class DoctorsRepository : IDoctorsRepository
    {
        private readonly IDbConnection connection;

        public DoctorsRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Doctor>> GetBySpecialtyAsync(string specialty)
        {
            return (await this.connection.QueryAsync<Doctor>(
                @"select * from doctors where s.""Specialty"" = @specialty", 
                new { specialty }));
        }

        public async Task<IEnumerable<Doctor>> All()
        {
            return (await this.connection.QueryAsync<Doctor>(@"select * from doctors"));
        }

        public async Task<Doctor> GetAsync(long id)
        {
            return await this.connection.QueryFirstOrDefaultAsync<Doctor>(@"select * from doctors where ""Id"" = @id", new { id });
        }

        public async Task CreateAsync(Doctor doctor)
        {
            doctor.Id = await connection.ExecuteScalarAsync<long>(
                @"insert into doctors (""Specialty"", ""FirstName"", ""SecondName"", ""ThirdName"", ""Info"")
                    values (@Specialty, @FirstName, @SecondName, @ThirdName, @Info) returning ""Id""",
                doctor);
        }
    }
}
