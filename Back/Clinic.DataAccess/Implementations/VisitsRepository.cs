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

        public async Task<IList<Visit>> FromRange(string username, DateTime from, DateTime to)
        {
            return (await this.connection.QueryAsync<Visit>(@"select * from visits where ""Username"" = @username and ""DateTime"" between @from and @to", new { username, from, to })).ToList();
        }
    }
}
