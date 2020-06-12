using System;
using Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Sections.GetSectionDetails
{
    public class GetSectionDetailQuery : IRequest<SectionLookupModel>
    {
        public Guid Id { get; set; }
    }
}
