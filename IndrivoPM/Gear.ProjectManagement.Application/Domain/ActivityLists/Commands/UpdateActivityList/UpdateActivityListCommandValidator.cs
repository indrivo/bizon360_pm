using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityList
{
    public class UpdateActivityListCommandValidator : AbstractValidator<UpdateActivityListCommand>
    {
        public UpdateActivityListCommandValidator()
        {
            RuleFor(al => al.Id).NotNull().NotEmpty();

            RuleFor(pg => pg.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.DueDate)
                .WithMessage("The start date should be less or equal to the due one.");
        }
    }
}
