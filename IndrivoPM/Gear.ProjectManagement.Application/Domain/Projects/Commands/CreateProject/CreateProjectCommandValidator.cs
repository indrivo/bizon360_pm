using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(a => a.Name)
                .Matches(@"^[^\\\/\:\*\?\<\>\|""]+$")
                .WithMessage(@"Name should not contain special characters: '\', '/', ':', '*', '?', '<', '>' and "" symbol. ");

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(p => p.ProjectManagerId)
                .NotNull();
        }
    }
}