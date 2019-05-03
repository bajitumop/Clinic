namespace Clinic.DataAccess.Repositories
{
    using System.Threading.Tasks;
    using Domain;

    public interface IImagesRepository
    {
        Task<Image> GetAsync(long id);

        Task UpsertAsync(long doctorId, byte[] content, string format);
    }
}
