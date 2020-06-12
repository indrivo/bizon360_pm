using Gear.Domain.CrmEntities.Primary;
using Microsoft.EntityFrameworkCore;

namespace Gear.Domain.CrmInterfaces
{
    public interface ICrmContext
    {
        DbSet<ClientOrganization> ClientOrganizations { get; set; }

        DbSet<ClientContact> ClientContacts { get; set; }

        DbSet<DealPipeline> Pipelines { get; set; }

        DbSet<DealStage> Stages { get; set; }

    }
}
