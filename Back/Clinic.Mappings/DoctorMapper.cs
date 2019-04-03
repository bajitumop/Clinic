namespace Clinic.Mappings
{
    using AutoMapper;

    using Domain;
    using Models.Doctors;

    public class DoctorMapper : Profile
    {
        public DoctorMapper()
        {
            this.CreateMap<Doctor, DoctorShortModel>()
                .ForMember(
                    model => model.ImageUrl,
                    options => options.MapFrom(d => d.ImageId.HasValue ? $"/api/images/doctors/{d.ImageId.Value}" : null));

            this.CreateMap<Doctor, DoctorModel>().ForMember(
                model => model.ImageUrl,
                options => options.MapFrom(d => d.ImageId.HasValue ? $"/api/images/doctors/{d.ImageId.Value}" : null));
        }
    }
}
