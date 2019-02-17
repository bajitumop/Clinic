namespace Clinic.DataAccess.DbSetConfigurations
{
    using System;
    using System.Collections.Generic;

    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using Newtonsoft.Json;

    public class DoctorsConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.Property(e => e.Schedule).HasConversion(
                s => JsonConvert.SerializeObject(s, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                s => JsonConvert.DeserializeObject<Dictionary<Specialty, Dictionary<DayOfWeek, TimeSpan[]>>>(s, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
