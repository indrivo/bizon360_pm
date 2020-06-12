using System;
using Gear.Sstp.Abstractions.Domain;

namespace Gear.Domain.PmInterfaces
{
    public interface IPmSstp
    {
        Guid? ServiceTypeId { get; set; }

        ServiceType ServiceTypes { get; set; }

        Guid? SolutionTypeId { get; set; }

        SolutionType SolutionTypes { get; set; }

        Guid? ProductTypeId { get; set; }

        ProductType ProductTypes { get; set; }

        Guid? TechnologyTypeId { get; set; }

        TechnologyType TechnologyTypes { get; set; }
    }
}
