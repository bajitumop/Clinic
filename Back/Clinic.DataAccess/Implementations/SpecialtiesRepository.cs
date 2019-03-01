namespace Clinic.DataAccess.Implementations
{
    using Clinic.DataAccess.Repositories;
    using Clinic.Domain;

    public class SpecialtiesRepository : DataRepository<Specialty>, ISpecialtiesRepository
    {
        public SpecialtiesRepository(DataContext dataContext)
            : base(dataContext)
        {
        }
    }
}
