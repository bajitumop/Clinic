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

    public class VisitsRepository : IVisitsRepository
    {
        private readonly IDbConnection connection;

        public VisitsRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IList<Visit>> ByUser(string username, DateTime from, DateTime to)
        {
            return (await this.connection.QueryAsync<Visit>(@"select * from visits where ""Username"" = @username and ""DateTime"" between @from and @to", new { username, from, to })).ToList();
        }

        public async Task<IList<Visit>> ByDoctor(long doctorId, DateTime from, DateTime to)
        {
            return (await this.connection.QueryAsync<Visit>(@"select * from visits where ""DateTime"" between @from and @to and ""DoctorId""=@doctorId", new { from, to, doctorId })).ToList();
        }

        public async Task Create(string username, long serviceId, long doctorId, DateTime dateTime)
        {
            await this.connection.ExecuteScalarAsync<long>(@"insert into visits (""Username"", ""ServiceId"", ""DoctorId"", ""DateTime"", ""Status"") VALUES(@username, @serviceId, @doctorId, @dateTime, @status) returning ""Id""", 
                new { username, serviceId, doctorId, dateTime, status = VisitStatus.Closed });
        }

        public async Task Delete(string username, long id)
        {
            await this.connection.ExecuteAsync(@"delete from visits where ""Id"" = @id and ""Username"" = @username", new { id, username });
        }
    }
}
