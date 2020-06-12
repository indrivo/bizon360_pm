using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Commands.UpdateSprintCommand
{
    public class UpdateSprintCommandValidator : AbstractValidator<UpdateSprintCommand>
    {
        public UpdateSprintCommandValidator()
        {
            RuleFor(s => s.Id).NotNull().NotEmpty();

            RuleFor(s => s.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(s => s.StartDate)
                .NotEmpty();

            RuleFor(s => s.EndDate)
                .NotEmpty();
        }
    }
}
