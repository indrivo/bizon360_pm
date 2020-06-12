using System;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList
{
    public class GetHeadlineListQuery : IRequest<HeadlineListViewModel>
    {
        public Guid ProjectId { get; set; }
    }
}
