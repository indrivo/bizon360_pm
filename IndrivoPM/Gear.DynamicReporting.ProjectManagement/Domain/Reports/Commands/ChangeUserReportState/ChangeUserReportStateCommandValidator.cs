using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.ChangeUserreportState
{
    public class ChangeUserReportStateCommandValidator : AbstractValidator<ChangeUserReportStateCommand>
    {
        public ChangeUserReportStateCommandValidator()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
