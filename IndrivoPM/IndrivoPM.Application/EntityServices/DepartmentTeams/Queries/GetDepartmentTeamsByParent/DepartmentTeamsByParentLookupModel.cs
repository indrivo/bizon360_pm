using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.HrmEntities;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsByParent
{
    public class DepartmentTeamsByParentLookup
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Abbreviation { get; set; } = "-";

        public bool Active { get; set; }

        public Guid? TeamLeadId { get; set; }

        public string TeamLeadFullName { get; set; }

        public Dictionary<Guid, string> Members { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }

        public int RowOrder { get; set; }


        public static Expression<Func<DepartmentTeam, DepartmentTeamsByParentLookup>> Projection
        {
            get
            {
                return team => new DepartmentTeamsByParentLookup
                {
                    Id = team.Id,
                    Name = team.Name,
                    Active = team.Active,
                    Abbreviation = team.Abbreviation,
                    TeamLeadId = team.DepartmentTeamLeadId,
                    TeamLeadFullName = team.DepartmentTeamLead != null ?
                        team.DepartmentTeamLead.FirstName + " " + team.DepartmentTeamLead.LastName : "Not Specified",
                    Members = team.UserDepartmentTeams.Select(x => x.User).ToDictionary(x => x.Id, x => x.FirstName + " " + x.LastName),
                    CreatedTime = team.CreatedTime,
                    ModifiedTime = team.ModifyTime,
                    RowOrder = team.RowOrder
                };
            }
        }

        public static DepartmentTeamsByParentLookup Create(DepartmentTeam team)
        {
            return Projection.Compile().Invoke(team);
        }
    }
}
