using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.TableHeaders.Commands.CreateTableHeader
{
    public class CreateTableHeaderCommandValidator : AbstractValidator<CreateTableHeaderCommand>
    {
        public CreateTableHeaderCommandValidator()
        {
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
