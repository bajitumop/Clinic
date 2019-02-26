namespace Clinic.DataAccess
{
    using System.Linq;

    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Visit> Visits { get; set; }

        public DbSet<User> Users { get; set; }
        
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

            modelBuilder.Entity<User>().Property(u => u.Permissions).HasConversion(
                permissions => JsonConvert.SerializeObject(permissions.Select(p => $"{p:G}")),
                dbValue => JsonConvert.DeserializeObject<UserPermission[]>(dbValue));
        }
    }
}
