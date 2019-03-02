namespace Clinic.DataAccess
{
    using System;
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

        public DbSet<Image> Images { get; set; }
        
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
            modelBuilder.Entity<Schedule>().Property(u => u.Weekdays).HasConversion(
                weekdays => JsonConvert.SerializeObject(weekdays),
                dbValue => JsonConvert.DeserializeObject<DateTime[]>(dbValue));
        }
    }
}
