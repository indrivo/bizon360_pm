using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectTabsNav
{
    public class GetProjectTabsNavCommand : IRequest<ProjectTabModel>
    {
        public Guid Id { get; set; }

        public bool IsUserPm { get; set; }
    }
}
