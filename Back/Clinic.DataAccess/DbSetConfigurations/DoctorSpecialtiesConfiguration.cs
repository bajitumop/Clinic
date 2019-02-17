namespace Clinic.DataAccess.DbSetConfigurations
{
    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DoctorSpecialtiesConfiguration : IEntityTypeConfiguration<DoctorSpecialty>
    {
        public void Configure(EntityTypeBuilder<DoctorSpecialty> builder)
        {
            builder.HasKey(ds => new { ds.DoctorId, ds.SpecialtyId });
            builder.HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSpecialties)
                .HasForeignKey(ds => ds.DoctorId);
            builder.HasOne(ds => ds.Specialty)
                .WithMany(s => s.DoctorSpecialties)
                .HasForeignKey(ds => ds.SpecialtyId);
        }
    }
}
