using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.DeactivateUserReportHeader
{
    public class DeactivateUserReportHeaderCommandValidator<TUserIdType> : AbstractValidator<DeactivateUserReportHeaderCommand<TUserIdType>>
    {
        public DeactivateUserReportHeaderCommandValidator()
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
