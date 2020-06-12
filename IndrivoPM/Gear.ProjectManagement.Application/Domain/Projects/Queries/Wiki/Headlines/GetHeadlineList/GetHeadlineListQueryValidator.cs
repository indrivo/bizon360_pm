using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList
{
    public class GetHeadlineListQueryValidator : AbstractValidator<GetHeadlineListQuery>
    {
        public GetHeadlineListQueryValidator()
        {
            RuleFor(request => request.ProjectId).NotNull().NotEmpty();
        }
    }
}
