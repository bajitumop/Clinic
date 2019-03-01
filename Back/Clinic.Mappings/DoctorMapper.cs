namespace Clinic.Mappings
{
    using AutoMapper;

    using Clinic.Domain;
    using Clinic.Models.Doctors;

    public class DoctorMapper : Profile
    {
        public DoctorMapper()
        {
            this.CreateMap<Doctor, DoctorShortModel>();
            this.CreateMap<Doctor, DoctorModel>();
        }
    }
}
