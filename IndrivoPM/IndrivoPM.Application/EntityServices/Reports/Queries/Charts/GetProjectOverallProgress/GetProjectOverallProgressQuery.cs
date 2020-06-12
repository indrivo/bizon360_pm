using System;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectOverallProgress
{
    public class GetProjectOverallProgressQuery : IRequest<ProjectOverallView>
    {
        public Guid ProjectId { get; set; }
    }
}
