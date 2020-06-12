using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.DeleteCheckItem
{
    public class DeleteCheckItemCommandValidator : AbstractValidator<DeleteCheckItemCommand>
    {
        public DeleteCheckItemCommandValidator()
        {
            RuleFor(x => x.CheckItemId)
                .NotEmpty()
                .NotNull();
        }
    }
}
