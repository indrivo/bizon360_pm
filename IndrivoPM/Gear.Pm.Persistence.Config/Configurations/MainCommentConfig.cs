using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class MainCommentConfig : IEntityTypeConfiguration<MainComment>
    {
        public void Configure(EntityTypeBuilder<MainComment> builder)
        {
            builder.HasIndex(x => x.RecordId);
        }
    }
}
