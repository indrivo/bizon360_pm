using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineDetails
{
    public class GetHeadlineDetailQueryValidator : AbstractValidator<GetHeadlineDetailQuery>
    {
        public GetHeadlineDetailQueryValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
