using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.DeactivateTableHeader
{
    public class DeactivateTableHeaderCommandValidator : AbstractValidator<DeactivateTableHeaderCommand>
    {
        public DeactivateTableHeaderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
