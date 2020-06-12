using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Reports.Queries.Charts.GetProjectOverallProgress
{
    public class GetProjectOverallProgressQueryValidator : AbstractValidator<GetProjectOverallProgressQuery>
    {
        public GetProjectOverallProgressQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
