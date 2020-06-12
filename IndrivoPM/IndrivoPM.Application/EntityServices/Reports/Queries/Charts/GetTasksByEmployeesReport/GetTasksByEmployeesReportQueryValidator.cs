using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetTasksByEmployeesReport
{
    public class GetTasksByEmployeesReportQueryValidator : AbstractValidator<GetTasksByEmployeesReportQuery>
    {
        public GetTasksByEmployeesReportQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
