using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(a => a.Name)
                .Matches(@"^[^\\\/\:\*\?\<\>\|""]+$")
                .WithMessage(@"Name should not contain special characters: '\', '/', ':', '*', '?', '<', '>' and "" symbol. ");

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.ProjectUrl)
                .MaximumLength(100);

            RuleFor(p => p.Budget)
                .GreaterThanOrEqualTo(0);

            RuleFor(p => p.ProjectManagerId)
                .NotNull();
        }
    }
}