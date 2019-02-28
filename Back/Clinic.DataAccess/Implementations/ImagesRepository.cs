namespace Clinic.DataAccess.Implementations
{
    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;

    public class ImagesRepository : DataRepository<Image>, IImagesRepository
    {
        public ImagesRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
