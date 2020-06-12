using Gear.Sstp.Abstractions.Domain;
using Gear.Sstp.Abstractions.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Gear.Sstp.Abstractions.Infrastructure
{
    public static class SstpRepoServiceResolver
    {
        public static IServiceCollection SstpRepoResolver(this IServiceCollection services)
        {
            services.AddTransient<ISstpRepo<ProductType>, SstpRepo<ProductType>>();
            services.AddTransient<ISstpRepo<ServiceType>, SstpRepo<ServiceType>>();
            services.AddTransient<ISstpRepo<SolutionType>, SstpRepo<SolutionType>>();
            services.AddTransient<ISstpRepo<TechnologyType>, SstpRepo<TechnologyType>>();

            return services;
        }
    }
}
