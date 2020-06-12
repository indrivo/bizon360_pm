using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CompleteCheckItem
{
    class CompleteCheckWithoutLoggingTimeCommandValidator : AbstractValidator<CompleteCheckWithoutLoggingTimeCommand>
    {
        public CompleteCheckWithoutLoggingTimeCommandValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();

            RuleFor(c => c.ActivityId).NotNull().NotEmpty();
        }
    }
}
