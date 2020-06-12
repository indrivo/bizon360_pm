using AutoMapper;

namespace Bizon360.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationToViewModelProfile());
            });
        }
    }
}
