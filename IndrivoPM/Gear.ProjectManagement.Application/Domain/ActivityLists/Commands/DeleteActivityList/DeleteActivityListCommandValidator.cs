using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.DeleteActivityList
{
    public class DeleteActivityListCommandValidator : AbstractValidator<DeleteActivityListCommand>
    {
        public DeleteActivityListCommandValidator()
        {
            RuleFor(al => al.Id).NotEmpty();
        }
    }
}
