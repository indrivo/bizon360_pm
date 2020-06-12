using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.UpdateUserReportGuids
{
    public class UpdateUserReportGuidsCommandValidator<TUserIdType> : AbstractValidator<UpdateUserReportGuidsCommand<TUserIdType>>
    {
        public UpdateUserReportGuidsCommandValidator()
        {
            RuleFor(x => x.ReportId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
