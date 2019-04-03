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

        public async Task<Schedule> GetAsync(long doctorId, string specialty)
        {
            return await this.connection.QueryFirstOrDefaultAsync<Schedule>(
                @"select * from schedules where ""DoctorId"" = @doctorId and ""Specialty"" = @specialty limit 1",
                new { doctorId, specialty });
        }

        public async Task UpsertAsync(Schedule schedule)
        {
            await this.connection.ExecuteAsync(@"
                insert into schedules (""DoctorId"", ""Specialty"", ""MondayStart"", ""MondayEnd"", ""TuesdayStart"", ""TuesdayEnd"",
                    ""WednesdayStart"", ""WednesdayEnd"", ""ThursdayStart"", ""ThursdayEnd"", ""FridayStart"", ""FridayEnd"",
                    ""SaturdayStart"", ""SaturdayEnd"", ""VisitDuration"")
                    values (@DoctorId, @Specialty, @MondayStart, @MondayEnd, @TuesdayStart, @TuesdayEnd,
                    @WednesdayStart, @WednesdayEnd, @ThursdayStart, @ThursdayEnd, @FridayStart, @FridayEnd,
                    @SaturdayStart, @SaturdayEnd, @VisitDuration)
                on conflict on constraint schedules_pkey do update set 
                    ""DoctorId"" = @doctorId,
                    ""Specialty"" = @Specialty,
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
                    ""SaturdayEnd"" = @SaturdayEnd,
                    ""VisitDuration"" = @VisitDuration
            ", schedule);
        }

        public async Task Delete(long doctorId, string specialty)
        {
            await this.connection.ExecuteAsync(@"delete from schedules where ""DoctorId"" = @doctorId and ""Specialty"" = @specialty", new {doctorId, specialty});
        }

        public async Task<List<Schedule>> All()
        {
            return (await this.connection.QueryAsync<Schedule>(@"select * from schedules")).ToList();
        }

        public async Task<List<Schedule>> GetByDoctorAsync(long doctorId)
        {
            return (await this.connection.QueryAsync<Schedule>(
                @"select * from schedules where ""DoctorId"" = @doctorId",
                new {doctorId})).ToList();
        }
    }
}
