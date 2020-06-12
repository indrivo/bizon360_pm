using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintStatusCommand
{
    public class UpdateSprintStatusCommandValidator : AbstractValidator<UpdateSprintStatusCommand>
    {
        public UpdateSprintStatusCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(a => a.Status)
                .NotNull();
        }
    }
}
