namespace Clinic.DataAccess
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // todo: apply di
            modelBuilder.ApplyConfiguration(new DoctorsConfiguration());
            modelBuilder.ApplyConfiguration(new DoctorSpecialtiesConfiguration());
        }
    }
}
