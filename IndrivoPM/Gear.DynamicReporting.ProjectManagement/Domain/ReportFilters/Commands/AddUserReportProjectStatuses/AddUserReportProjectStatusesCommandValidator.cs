using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportProjectStatuses
{
    public class AddUserReportProjectStatusesCommandValidator<TUserIdType> : AbstractValidator<AddUserReportProjectStatusesCommand<TUserIdType>>
    {
        public AddUserReportProjectStatusesCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();

            RuleFor(x => x.ProjectStatuses)
                .NotEmpty();
        }
    }
}
