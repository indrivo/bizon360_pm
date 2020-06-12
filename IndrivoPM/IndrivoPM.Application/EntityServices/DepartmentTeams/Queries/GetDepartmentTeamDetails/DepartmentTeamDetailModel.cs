using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamDetails
{
    public class DepartmentTeamDetailModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public string Abbreviation { get; set; }

        public string Description { get; set; }

        public Guid? DepartmentTeamLeadId { get; set; }

        public string TeamLeadFullName { get; set; }

        public string TeamLeadJobPosition { get; set; }

        public bool Active { get; set; }

        public List<Guid> MembersIds { get; set; }

        public List<ApplicationUserLookupModel> Members { get; set; }

        public List<Guid> JobPositionIds { get; set; }

        public List<JobPositionLookupModel> JobPositions { get; set; }


        public static Expression<Func<DepartmentTeam, DepartmentTeamDetailModel>> Projection
        {
            get
            {
                return team => new DepartmentTeamDetailModel
                {
                    Id = team.Id,
                    Name = team.Name,
                    Active = team.Active,
                    Abbreviation = team.Abbreviation,
                    Description = team.Description,
                    DepartmentId = team.DepartmentId ?? null,
                    DepartmentTeamLeadId = team.DepartmentTeamLeadId,
                    DepartmentName = team.Department != null && team.Department.Name != null ? team.Department.Name : "",
                    Members = team.UserDepartmentTeams.Select(x => ApplicationUserLookupModel.Create(x.User)).ToList(),
                    MembersIds = team.UserDepartmentTeams.Select(x => x.UserId).ToList(),
                    JobPositionIds = team.JobDepartmentTeams.Select(x => x.JobPositionId).ToList(),
                    JobPositions = team.JobDepartmentTeams.Select(x => JobPositionLookupModel.Create(x.JobPosition)).ToList(),
                    TeamLeadFullName = team.DepartmentTeamLead != null ? (team.DepartmentTeamLead.FirstName +" "+ team.DepartmentTeamLead.LastName) : "Not Specified",
                    TeamLeadJobPosition  = team.DepartmentTeamLead != null ? team.DepartmentTeamLead.JobPosition != null ? team.DepartmentTeamLead.JobPosition.Name : "" : ""
                };
            }
        }
        public static DepartmentTeamDetailModel Create(DepartmentTeam team)
        {
            return Projection.Compile().Invoke(team);
        }
    }
}
