using Gear.Sstp.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;

namespace Gear.Sstp.Abstractions.Service
{
    public interface ISstpContext
    {
        /// <summary>
        /// For built in generic methods
        /// </summary>
        DbContext Instance { get; }

        DbSet<ServiceType> ServiceTypes { get; set; }

        DbSet<SolutionType> SolutionTypes { get; set; }

        DbSet<TechnologyType> TechnologyTypes { get; set; }

        DbSet<ProductType> ProductTypes { get; set; }

    }
}
