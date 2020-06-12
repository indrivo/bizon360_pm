using System.Collections.Generic;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupList
{
    public class GetProjectGroupListQuery : IRequest<ProjectGroupListViewModel>
    {
        public bool HasAccessToAllEntities { get; set; }

        public ICollection<ProjectStatus> Filter { get; set; }
    }
}
