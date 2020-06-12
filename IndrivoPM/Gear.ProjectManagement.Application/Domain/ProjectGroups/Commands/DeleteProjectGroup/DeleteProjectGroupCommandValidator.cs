using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ProjectGroups.Commands.DeleteProjectGroup
{
    public class DeleteProjectGroupCommandValidator : AbstractValidator<DeleteProjectGroupCommand>
    {
        public DeleteProjectGroupCommandValidator()
        {
            RuleFor(pg => pg.Id).NotEmpty();
        }
    }
}
