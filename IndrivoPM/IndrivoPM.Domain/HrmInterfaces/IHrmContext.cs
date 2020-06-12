using Gear.Domain.HrmEntities;
using Gear.Domain.HrmEntities.Recruitment;
using Microsoft.EntityFrameworkCore;

namespace Gear.Domain.HrmInterfaces
{
    public interface IHrmContext
    {
        DbContext Instance { get; }

        DbSet<Department> Departments { get; set; }

        DbSet<BusinessUnit> BusinessUnits { get; set; }

        DbSet<DepartmentTeam> DepartmentTeams { get; set; }

        DbSet<UserDepartmentTeam> UserDepartmentTeams { get; set; }

        DbSet<JobDepartmentTeam> JobDepartmentTeams { get; set; }

        DbSet<JobPosition> JobPositions { get; set; }

        DbSet<Candidate> Candidates { get; set; }

        DbSet<RecruitmentPipeline> RecruitingPipelines { get; set; }

        DbSet<RecruitmentStage> RecruitmentStages { get; set; }
    }
}
