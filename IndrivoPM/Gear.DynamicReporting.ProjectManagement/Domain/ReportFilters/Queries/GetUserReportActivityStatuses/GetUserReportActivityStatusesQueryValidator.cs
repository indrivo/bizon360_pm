using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Gear.DynamicReporting.ProjectManagement.Domain.ReportFilters.Queries.GetUserReportActivityStatuses
{
    class GetUserReportActivityStatusesQueryValidator : AbstractValidator<GetUserReportActivityStatusesQuery>
    {
        public GetUserReportActivityStatusesQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ReportId)
                .NotEmpty();
        }
    }
}
