using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.BulkActions.ChangeProjectActivityTypeStates
{
    public class ChangeProjectActivityTypeStatesCommandValidator : AbstractValidator<ChangeProjectActivityTypeStatesCommand>
    {
        public ChangeProjectActivityTypeStatesCommandValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            RuleFor(x => x.ActivityTypeIds)
                .NotEmpty();
        }
    }
}
