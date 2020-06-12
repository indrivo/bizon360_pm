using System;
using System.Collections.Generic;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityListByAssignee
{
    public class GetActivityListByAssigneeQuery : IRequest<AssigneeListViewModel>
    {
        public Guid ProjectId { get; set; }

        public ICollection<string> OpenedCollapsesById { get; set; }
    }
}
