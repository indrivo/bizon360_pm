using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.ActivateTableHeader
{
    public class ActivateTableHeaderCommandValidator : AbstractValidator<ActivateTableHeaderCommand>
    {
        public ActivateTableHeaderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
