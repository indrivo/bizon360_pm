using Gear.Domain.HrmEntities.Recruitment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gear.Hrm.Persistence.Config.Configurations
{
    public class CandidateConfig : BaseModelConfig<Candidate>
    {
        public override void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.Ignore(x => x.Name);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.DesiredSalary)
                .HasColumnType(typeName: "MONEY");

            builder.HasOne(x => x.JobPosition);

            builder.OwnsOne(x => x.Name);

            builder.OwnsOne(x => x.ContactInfo);
        }
    }
}
