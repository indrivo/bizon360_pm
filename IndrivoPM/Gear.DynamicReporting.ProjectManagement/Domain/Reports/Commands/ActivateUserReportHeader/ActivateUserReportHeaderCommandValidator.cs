using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.ActivateUserReportHeader
{
    public class ActivateUserReportHeaderCommandValidator<TUserIdType> : AbstractValidator<ActivateUserReportHeaderCommand<TUserIdType>>
    {
        public ActivateUserReportHeaderCommandValidator()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.TableHeaderId)
                .NotEmpty();
        }
    }
}
