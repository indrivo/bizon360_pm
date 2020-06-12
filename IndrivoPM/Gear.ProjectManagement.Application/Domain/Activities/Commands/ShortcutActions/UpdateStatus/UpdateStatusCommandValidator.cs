using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.UpdateStatus
{
    public class UpdateStatusCommandValidator : AbstractValidator<UpdateStatusCommand>
    {
        public UpdateStatusCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();

            RuleFor(c => c.Status).NotNull().NotEmpty();
        }
    }
}
