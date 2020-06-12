using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.UpdateReport
{
    public class UpdateReportCommandValidator : AbstractValidator<UpdateReportCommand>
    {
        public UpdateReportCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
