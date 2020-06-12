using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.AddTableHeaderToUserReport
{
    public class AddTableHeaderToUserReportCommandValidator<TUserIdType> : AbstractValidator<AddTableHeaderToUserReportCommand<TUserIdType>>
    {
        public AddTableHeaderToUserReportCommandValidator()
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
