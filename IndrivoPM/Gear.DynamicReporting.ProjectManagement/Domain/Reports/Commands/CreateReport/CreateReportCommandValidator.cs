using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.CreateReport
{
    public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
    {
        public CreateReportCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
