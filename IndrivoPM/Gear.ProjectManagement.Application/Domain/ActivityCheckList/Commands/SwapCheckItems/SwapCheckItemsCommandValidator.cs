using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.SwapCheckItems
{
    public class SwapCheckItemsCommandValidator : AbstractValidator<SwapCheckItemsCommand>
    {
        public SwapCheckItemsCommandValidator()
        {
            RuleFor(x => x.Ids)
                .NotEmpty();
        }
    }
}
