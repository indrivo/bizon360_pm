using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetProjectActivityTypes
{
    public class GetProjectActivityTypesQuery : IRequest<ProjectActivityTypeListViewModel>
    {
        public Guid ProjectId { get; set; }
    }
}
