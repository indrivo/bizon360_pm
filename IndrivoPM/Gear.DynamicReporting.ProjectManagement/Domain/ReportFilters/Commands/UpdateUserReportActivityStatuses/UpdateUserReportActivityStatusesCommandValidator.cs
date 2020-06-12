using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportActivityStatuses
{
    public class UpdateUserReportActivityStatusesCommandValidator<TUserIdType> : AbstractValidator<UpdateUserReportActivityStatusesCommand<TUserIdType>>
    {
        public UpdateUserReportActivityStatusesCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
