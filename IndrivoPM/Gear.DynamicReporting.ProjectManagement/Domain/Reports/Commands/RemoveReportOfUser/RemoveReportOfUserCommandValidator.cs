using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.RemoveReportOfUser
{
    public class RemoveReportOfUserCommandValidator<TUserIdType> : AbstractValidator<RemoveReportOfUserCommand<TUserIdType>>
    {
        public RemoveReportOfUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
