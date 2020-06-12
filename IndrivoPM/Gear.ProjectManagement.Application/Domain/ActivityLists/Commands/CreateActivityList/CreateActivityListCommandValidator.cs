using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.CreateActivityList
{
    public class CreateActivityListCommandValidator : AbstractValidator<CreateActivityListCommand>
    {
        public CreateActivityListCommandValidator()
        {
            RuleFor(al => al.Id).NotNull().NotEmpty();

            RuleFor(al => al.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(al => al.ProjectId).NotNull().NotEmpty();
        }
    }
}
