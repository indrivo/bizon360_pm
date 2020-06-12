using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.Reports.Commands.AddUserToReport
{
    public class AddUserToReportCommandValidator<TUserIdType> : AbstractValidator<AddUserToReportCommand<TUserIdType>>
    {
        public AddUserToReportCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
