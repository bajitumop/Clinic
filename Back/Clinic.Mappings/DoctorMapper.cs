namespace Clinic.Mappings
{
    using AutoMapper;

    using Domain;
    using Models.Doctors;

    public class DoctorMapper : Profile
    {
        public DoctorMapper()
        {
            this.CreateMap<Doctor, DoctorModel>();
        }
    }
}
