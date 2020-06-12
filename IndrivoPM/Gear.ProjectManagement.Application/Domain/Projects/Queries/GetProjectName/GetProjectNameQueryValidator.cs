using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectName
{
    public class GetProjectNameQueryValidator : AbstractValidator<GetProjectNameQuery>
    {
        public GetProjectNameQueryValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}