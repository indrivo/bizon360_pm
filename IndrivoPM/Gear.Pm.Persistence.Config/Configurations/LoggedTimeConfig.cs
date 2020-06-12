using Gear.Domain.PmEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Pm.Persistence.Config.Configurations
{
    public class LoggedTimeConfig : IEntityTypeConfiguration<LoggedTime>
    {
        public void Configure(EntityTypeBuilder<LoggedTime> builder)
        {
            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.TrackerId).IsRequired();

            builder.Property(x => x.Time).IsRequired();

            builder.Property(x => x.DateOfWork).IsRequired();
        }
    }
}
