using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.UpdateActivity
{
    public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
    {
        public UpdateActivityCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(a => a.Name)
                .Matches(@"^[^\\\/\:\*\?\<\>\|""]+$")
                .WithMessage(@"Name should not contain special characters: '\', '/', ':', '*', '?', '<', '>' and "" symbol. ");

            RuleFor(a => a.Assignees).NotNull().NotEmpty();

            RuleFor(a => a.EstimatedHours)
                .GreaterThanOrEqualTo(0);
        }
    }
}
