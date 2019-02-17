namespace Clinic.DataAccess
{
    using Clinic.DataAccess.DbSetConfigurations;
    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ToDo: Apply DI
            modelBuilder.ApplyConfiguration(new DoctorsConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorSpecialtiesConfiguration());
        }
    }
}
