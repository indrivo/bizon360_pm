using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.UpdateProjectGroup
{
    public class UpdateProjectGroupCommandValidator : AbstractValidator<UpdateProjectGroupCommand>
    {
        public UpdateProjectGroupCommandValidator()
        {
            RuleFor(pg => pg.Id).NotEmpty();

            RuleFor(pg => pg.Name)
                .NotEmpty()
                .MaximumLength(256);
        }
    }
}
