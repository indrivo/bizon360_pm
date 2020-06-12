using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSettings
{
    public class GetProjectSettingsQuery : IRequest<ProjectSettingsModel>
    {
        public Guid ProjectId { get; set; }
    }
}
