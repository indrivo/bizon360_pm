using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.ShortcutActions.RenameActivity
{
    public class RenameActivityCommandValidator : AbstractValidator<RenameActivityCommand>
    {
        public RenameActivityCommandValidator()
        {
            RuleFor(a => a.Id).NotNull().NotEmpty();

            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(a => a.Name)
                .Matches(@"^[^\\\/\:\*\?\<\>\|""]+$")
                .WithMessage(@"Name should not contain special characters: '\', '/', ':', '*', '?', '<', '>' and "" symbol. ");
        }
    }
}
