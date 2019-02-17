namespace Clinic.DataAccess
{
    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>().HasKey(c => new { c.DoctorId, c.SpecialtyId });
            modelBuilder.Entity<Schedule>()
                .HasOne(sc => sc.Doctor)
                .WithMany(d => d.Schedules)
                .HasForeignKey(sc => sc.DoctorId);
            modelBuilder.Entity<Schedule>()
                .HasOne(sc => sc.Specialty)
                .WithMany(d => d.Schedules)
                .HasForeignKey(sc => sc.SpecialtyId);
        }
    }
}
