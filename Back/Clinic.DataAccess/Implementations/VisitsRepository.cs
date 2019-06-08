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

        public async Task<IList<Visit>> All(DateTime from, DateTime to)
        {
            return (await this.connection.QueryAsync<Visit>(@"select * from visits where ""DateTime"" between @from and @to", new { from, to })).ToList();
        }

        public async Task Create(string username, long serviceId, long doctorId, DateTime dateTime)
        {
            await this.connection.ExecuteScalarAsync<long>(@"insert into visits (""Username"", ""ServiceId"", ""DoctorId"", ""DateTime"", ""VisitStatus"") VALUES(@username, @serviceId, @doctorId, @dateTime, @visitStatus) returning ""Id""", 
                new { username, serviceId, doctorId, dateTime, visitStatus = VisitStatus.Opened });
        }

        public async Task Delete(string username, long id)
        {
            await this.connection.ExecuteAsync(@"delete from visits where ""Id"" = @id and ""Username"" = @username", new { id, username });
        }
    }
}
