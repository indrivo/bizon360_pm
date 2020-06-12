using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Commands.AddUserReportGuids
{
    public class AddUserReportGuidsCommandValidator<TUserIdType> : AbstractValidator<AddUserReportGuidsCommand<TUserIdType>>
    {
        public AddUserReportGuidsCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.GuidList)
                .NotEmpty();
        }
    }
}
