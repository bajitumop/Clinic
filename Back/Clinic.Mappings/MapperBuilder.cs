namespace Clinic.Mappings
{
    using System;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;

    public class MapperBuilder
    {
        public static IMapper Build()
        {
            var mapperConfiguration = new MapperConfiguration(mc =>
                    {
                        mc.AddProfiles(Assembly.GetAssembly(typeof(MapperBuilder)));
                    });

            return mapperConfiguration.CreateMapper();
        }
    }
}
