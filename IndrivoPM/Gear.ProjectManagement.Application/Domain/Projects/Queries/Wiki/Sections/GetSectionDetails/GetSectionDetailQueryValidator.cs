using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Sections.GetSectionDetails
{
    public class GetSectionDetailQueryValidator : AbstractValidator<GetSectionDetailQuery>
    {
        public GetSectionDetailQueryValidator()
        {
            RuleFor(s => s.Id).NotNull().NotEmpty();
        }
    }
}
