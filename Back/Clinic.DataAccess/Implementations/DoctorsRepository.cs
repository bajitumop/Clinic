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

        public async Task<IList<Doctor>> GetBySpecialtyAsync(string specialty)
        {
            return (await this.connection.QueryAsync<Doctor>(
                @"select * from doctors as d
                     join schedules as s on d.""Id"" = s.""DoctorId""
                     where s.""Specialty"" = @specialty", 
                new { specialty }))
                .ToList();
        }

        public async Task<IList<Doctor>> All()
        {
            return (await this.connection.QueryAsync<Doctor>(@"select * from doctors")).ToList();
        }

        public async Task<Doctor> GetAsync(long id)
        {
            return await this.connection.QueryFirstOrDefaultAsync<Doctor>(@"select * from doctors where ""Id"" = @id", new { id });
        }

        public async Task CreateAsync(Doctor doctor)
        {
            doctor.Id = await connection.ExecuteScalarAsync<long>(
                @"insert into doctors (""FirstName"", ""SecondName"", ""ThirdName"", ""Info"", ""Positions"", ""DoctorPermission"")
                    values (@FirstName, @SecondName, @ThirdName, @Info, @Positions, @DoctorPermission) returning ""Id""",
                doctor);
        }

        public async Task Delete(long id)
        {
            await this.connection.ExecuteAsync(@"delete from images where ""Id"" = (select ""ImageId"" from doctors where ""Id"" = @id)", new {id});
            await this.connection.ExecuteAsync(@"delete from doctors where ""Id"" = @id", new {id});
        }
    }
}
