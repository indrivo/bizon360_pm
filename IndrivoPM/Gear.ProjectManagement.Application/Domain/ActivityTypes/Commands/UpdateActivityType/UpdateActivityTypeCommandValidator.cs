using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Commands.UpdateActivityType
{
    public class UpdateActivityTypeCommandValidator:AbstractValidator<UpdateActivityTypeCommand>
    {
        public UpdateActivityTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();
        }
    }
}
