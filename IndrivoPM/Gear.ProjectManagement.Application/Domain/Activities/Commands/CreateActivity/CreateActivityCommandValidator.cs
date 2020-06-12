using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Commands.CreateActivity
{
    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityCommandValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(a => a.Name)
                .Matches(@"^[^\\\/\:\*\?\<\>\|""]+$")
                .WithMessage(@"Name should not contain special characters: '\', '/', ':', '*', '?', '<', '>' and "" symbol. ");

            RuleFor(a => a.Assignees).NotNull().NotEmpty();

            RuleFor(a => a.EstimatedHours)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(a => a.Progress)
                .NotNull()
                .InclusiveBetween(0, 100);
        }
    }
}
