using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityCheckList.Commands.RenameCheckItem
{
    class RenameCheckItemCommandValidator : AbstractValidator<RenameCheckItemCommand>
    {
        public RenameCheckItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotNull()
                .NotEmpty();
        }
    }
}
