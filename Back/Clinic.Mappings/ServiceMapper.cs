namespace Clinic.Mappings
{
    using AutoMapper;

    using Clinic.Domain;
    using Clinic.Models;

    public class ServiceMapper : Profile
    {
        public ServiceMapper()
        {
            this.CreateMap<Service, ServiceModel>()
                .ForMember(model => model.SpecialtyId, options => options.MapFrom(s => s.Specialty.Id))
                .ForMember(model => model.SpecialtyName, options => options.MapFrom(s => s.Specialty.Name));
        }
    }
}
