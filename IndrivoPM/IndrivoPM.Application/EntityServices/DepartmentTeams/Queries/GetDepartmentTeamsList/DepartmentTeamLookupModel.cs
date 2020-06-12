using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;
using Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionList;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsList
{
    public class DepartmentTeamLookupModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Abbreviation { get; set; }

        public Guid? DepartmentId { get; set; }

        public bool Active { get; set; }

        public Guid? DepartmentTeamLeadId { get; set; }

        public string TeamLeadFullName { get; set; }

        public string TeamLeadJobPosition { get; set; }

        public string DepartmentName { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }
        
        public List<ApplicationUserLookupModel> Members { get; set; }


        public IList<JobPositionLookupModel> JobPositions { get; set; }


        public static Expression<Func<DepartmentTeam, DepartmentTeamLookupModel>> Projection
        {
            get
            {

                return depTeam => new DepartmentTeamLookupModel
                {
                    Id = depTeam.Id,
                    Name = depTeam.Name,
                    Description = depTeam.Description,
                    Active = depTeam.Active,
                    TeamLeadFullName = depTeam.DepartmentTeamLead != null ? (depTeam.DepartmentTeamLead.FirstName + " " + depTeam.DepartmentTeamLead.LastName) : "Not Specified",
                    DepartmentTeamLeadId = depTeam.DepartmentTeamLeadId,
                    DepartmentId = depTeam.DepartmentId,
                    DepartmentName = depTeam.Department != null ? depTeam.Department.IsDeletable == true ? depTeam.Department.Name : "" : "",
                    Abbreviation = depTeam.Abbreviation,
                    CreatedTime = depTeam.CreatedTime,
                    ModifiedTime = depTeam.ModifyTime,
                    //TODO: Check for null
                    JobPositions = depTeam.JobDepartmentTeams.Select(x => JobPositionLookupModel.Create(x.JobPosition)).ToList(),
                    TeamLeadJobPosition = depTeam.DepartmentTeamLead != null ? depTeam.DepartmentTeamLead.JobPosition !=null ? depTeam.DepartmentTeamLead.JobPosition.Name  : " None" : "None"
                };
            }
        }

        public static DepartmentTeamLookupModel Create(DepartmentTeam depTeam)
        {
            
            return Projection.Compile().Invoke(depTeam);
        }

    }
}
