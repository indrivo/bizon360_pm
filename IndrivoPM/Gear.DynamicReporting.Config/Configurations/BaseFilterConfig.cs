using System;
using Gear.Domain.ReportEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.DynamicReporting.Config.Configurations
{
    public class BaseFilterConfig<TClass> : IEntityTypeConfiguration<TClass> where TClass : BaseFilter<Guid>
    {
        public virtual void Configure(EntityTypeBuilder<TClass> builder)
        {
            builder.HasKey(key => key.Id);

            builder.HasOne(one => one.ApplicationUser)
                .WithMany()
                .HasForeignKey(fk => fk.UserId);
        }
    }
}
