using System;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineDetails
{
    public class GetHeadlineDetailQuery : IRequest<HeadlineLookupModel>
    {
        public Guid Id { get; set; }
    }
}
