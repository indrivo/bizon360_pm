using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.UpdateTableHeader
{
    public class UpdateTableHeaderCommandValidator : AbstractValidator<UpdateTableHeaderCommand>
    {
        public UpdateTableHeaderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Order)
                .GreaterThan(0);

            RuleFor(x => x.Width)
                .GreaterThan(0);
        }
    }
}
