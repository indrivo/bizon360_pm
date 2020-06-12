using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportActivityStatuses
{
    public class AddUserReportActivityStatusesCommandValidator<TUserIdType> : AbstractValidator<AddUserReportActivityStatusesCommand<TUserIdType>>
    {
        public AddUserReportActivityStatusesCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();

            RuleFor(x => x.ActivityStatuses)
                .NotEmpty();
        }
    }
}
