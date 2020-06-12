using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectDetails
{
    public class GetProjectDetailQueryValidator : AbstractValidator<GetProjectDetailQuery>
    {
        public GetProjectDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}