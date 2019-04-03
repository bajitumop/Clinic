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
            return await this.connection.QueryFirstOrDefaultAsync<Image>(@"select * from images where ""Id"" = @id", new { id });
        }

        public async Task UpsertAsync(long doctorId, byte[] content, string format)
        {
            var imageId = await this.connection.ExecuteScalarAsync<long>(
                @"insert into images (""Content"", ""Format"") values (@content, @format) returning ""Id""",
                new {content, format});

            var oldImageId = await this.connection.ExecuteScalarAsync<long?>(
                @"select ""ImageId"" from doctors where ""Id"" = @doctorId", 
                new {doctorId});

            await this.connection.ExecuteAsync(@"update doctors set ""ImageId"" = @imageId where ""Id"" = @doctorId", new { imageId, doctorId });

            if (oldImageId.HasValue)
            {
                await this.connection.ExecuteAsync(@"delete from images where ""Id"" = @oldImageId", new { oldImageId });
            }
        }

        public async Task DeleteAsync(long doctorId, long imageId)
        {
            await this.connection.ExecuteAsync(@"update doctors set ""ImageId"" = null where ""Id"" = @doctorId", new { doctorId });
            await this.connection.ExecuteAsync(@"delete from images where ""Id"" = @imageId", new { imageId });
        }
    }
}
