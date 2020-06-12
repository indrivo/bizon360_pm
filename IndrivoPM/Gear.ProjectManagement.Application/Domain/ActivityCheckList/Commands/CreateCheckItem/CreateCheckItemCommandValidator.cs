using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.CreateCheckItem
{
    public class CreateCheckItemCommandValidator : AbstractValidator<CreateCheckItemCommand>
    {
        public CreateCheckItemCommandValidator()
        {
            RuleFor(x => x.ActivityId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Content)
                .NotEmpty();

        }
    }
}
