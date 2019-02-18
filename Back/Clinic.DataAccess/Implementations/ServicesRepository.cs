namespace Clinic.DataAccess.Implementations
{
    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;

    public class ServicesRepository : DataRepository<Service>, IServicesRepository
    {
        public ServicesRepository(DataContext dataContext)
            : base(dataContext)
        {
        }
    }
}
