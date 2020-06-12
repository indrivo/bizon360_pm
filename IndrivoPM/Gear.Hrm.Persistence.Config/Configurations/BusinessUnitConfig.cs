using Gear.Domain.HrmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class BusinessUnitConfig : BaseModelConfig<BusinessUnit>
    {
        public override void Configure(EntityTypeBuilder<BusinessUnit> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Description)
                .HasMaxLength(900);

            builder.HasOne(x => x.BusinessUnitLead)
                .WithMany(x => x.BusinessUnits)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Department)
                .WithOne(x => x.BusinessUnit)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
