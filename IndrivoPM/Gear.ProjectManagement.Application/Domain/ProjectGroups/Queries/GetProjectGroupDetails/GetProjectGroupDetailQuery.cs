using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Queries.GetProjectGroupDetails
{
    public class GetProjectGroupDetailQuery : IRequest<ProjectGroupDetailModel>
    {
        public Guid Id { get; set; }
    }
}
