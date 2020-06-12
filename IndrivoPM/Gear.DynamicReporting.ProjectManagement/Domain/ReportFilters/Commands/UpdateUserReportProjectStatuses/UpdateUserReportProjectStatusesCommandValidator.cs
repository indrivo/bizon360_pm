using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportProjectStatuses
{
    public class UpdateUserReportProjectStatusesCommandValidator<TUserIdType> : AbstractValidator<UpdateUserReportProjectStatusesCommand<TUserIdType>>
    {
        public UpdateUserReportProjectStatusesCommandValidator()
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
