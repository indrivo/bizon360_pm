using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectTabsNav
{
    public class GetProjectTabsNavCommandValidator : AbstractValidator<GetProjectTabsNavCommand>
    {
        public GetProjectTabsNavCommandValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
