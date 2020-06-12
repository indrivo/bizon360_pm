using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.DeleteActivity
{
    public class DeleteActivityCommandValidator : AbstractValidator<DeleteActivityCommand>
    {
        public DeleteActivityCommandValidator()
        {
            RuleFor(a => a.Id).NotEmpty();
        }
    }
}
