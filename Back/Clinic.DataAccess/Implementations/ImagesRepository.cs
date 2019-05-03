namespace Clinic.DataAccess.Implementations
{
    using System.Data;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Dapper;
    using Domain;
    using Repositories;

    public class ImagesRepository : IImagesRepository
    {
        private readonly IDbConnection connection;

        public ImagesRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<Image> GetAsync(long id)
        {
            return await this.connection.QueryFirstOrDefaultAsync<Image>(@"select * from images where ""Id"" = @id limit 1", new { id });
        }

        public async Task UpsertAsync(long doctorId, byte[] content, string format)
        {
            await this.connection.ExecuteAsync(@"
                    insert into images (""Id"", ""Content"", ""Format"")
                    values (@doctorId, @content, @format)
                    on conflict on constraint images_pkey
                    do update set ""Content"" = @content, ""Format"" = @format",
                new {doctorId, content, format});
        }
    }
}
