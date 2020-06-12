using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.CreateActivityType
{
    public class CreateActivityTypeCommandValidator:AbstractValidator<CreateActivityTypeCommand>
    {
        public CreateActivityTypeCommandValidator()
        {

            RuleFor(x => x.Name).NotEmpty()
                .MaximumLength(256);

        }
    }
}
