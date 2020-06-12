using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.CreateProjectGroup
{
    public class CreateProjectGroupCommandValidator : AbstractValidator<CreateProjectGroupCommand>
    {
        public CreateProjectGroupCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(256);
        }
    }
}
