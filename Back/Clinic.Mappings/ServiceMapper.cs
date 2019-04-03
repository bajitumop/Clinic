namespace Clinic.Mappings
{
    using AutoMapper;

    using Clinic.Domain;
    using Clinic.Models;

    public class ServiceMapper : Profile
    {
        public ServiceMapper()
        {
            this.CreateMap<Service, ServiceModel>();
        }
    }
}
