using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.SetUpProject
{
    public class SetUpProjectCommandValidator : AbstractValidator<SetUpProjectCommand>
    {
        public SetUpProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
