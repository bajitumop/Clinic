namespace Clinic.DataAccess.Implementations
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Repositories;
    using Domain;
    
    public class SchedulesRepository : ISchedulesRepository
    {
        private readonly IDbConnection connection;

        public SchedulesRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Schedule> GetAsync(long doctorId)
        {
            return await this.connection.QueryFirstOrDefaultAsync<Schedule>(
                @"select * from schedules where ""DoctorId"" = @doctorId limit 1",
                new { doctorId });
        }

        public async Task UpsertAsync(Schedule schedule)
        {
            await this.connection.ExecuteAsync(@"
                insert into schedules (""DoctorId"", ""MondayStart"", ""MondayEnd"", ""TuesdayStart"", ""TuesdayEnd"", ""WednesdayStart"",
                    ""WednesdayEnd"", ""ThursdayStart"", ""ThursdayEnd"", ""FridayStart"", ""FridayEnd"", ""SaturdayStart"", ""SaturdayEnd"")
                    values (@DoctorId, @MondayStart, @MondayEnd, @TuesdayStart, @TuesdayEnd, @WednesdayStart, @WednesdayEnd, @ThursdayStart, 
                    @ThursdayEnd, @FridayStart, @FridayEnd, @SaturdayStart, @SaturdayEnd)
                on conflict on constraint schedules_pkey do update set 
                    ""MondayStart"" = @MondayStart,
                    ""MondayEnd"" = @MondayEnd,
                    ""TuesdayStart"" = @TuesdayStart,
                    ""TuesdayEnd"" = @TuesdayEnd,
                    ""WednesdayStart"" = @WednesdayStart,
                    ""WednesdayEnd"" = @WednesdayEnd,
                    ""ThursdayStart"" = @ThursdayStart,
                    ""ThursdayEnd"" = @ThursdayEnd,
                    ""FridayStart"" = @FridayStart,
                    ""FridayEnd"" = @FridayEnd,
                    ""SaturdayStart"" = @SaturdayStart,
                    ""SaturdayEnd"" = @SaturdayEnd
            ", schedule);
        }

        public async Task Delete(long doctorId)
        {
            await this.connection.ExecuteAsync(@"delete from schedules where ""DoctorId"" = @doctorId", new { doctorId });
        }

        public async Task<IEnumerable<Schedule>> All()
        {
            return await this.connection.QueryAsync<Schedule>(@"select * from schedules");
        }
    }
}
