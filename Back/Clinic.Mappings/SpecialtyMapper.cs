namespace Clinic.Mappings
{
    using AutoMapper;

    using Clinic.Domain;
    using Clinic.Models;

    public class SpecialtyMapper : Profile
    {
        public SpecialtyMapper()
        {
            this.CreateMap<Specialty, SpecialtyModel>();
        }
    }
}
