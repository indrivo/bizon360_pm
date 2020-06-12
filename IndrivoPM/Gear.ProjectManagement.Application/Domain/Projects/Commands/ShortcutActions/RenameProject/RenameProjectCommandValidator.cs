using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.ShortcutActions.RenameProject
{
    public class RenameProjectCommandValidator : AbstractValidator<RenameProjectCommand>
    {
        public RenameProjectCommandValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty();

            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(a => a.Name)
                .Matches(@"^[^\\\/\:\*\?\<\>\|""]+$")
                .WithMessage(@"Name should not contain special characters: '\', '/', ':', '*', '?', '<', '>' and "" symbol. ");
        }
    }
}
