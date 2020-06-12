using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSettings
{
    public class GetProjectSettingsQueryValidator : AbstractValidator<GetProjectSettingsQuery>
    {
        public GetProjectSettingsQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
