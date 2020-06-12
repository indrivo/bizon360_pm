using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Sprints.Queries.GetSprintDetailQuery
{
    public class GetSprintDetailQueryValidator : AbstractValidator<GetSprintDetailQuery>
    {
        public GetSprintDetailQueryValidator()
        {
            RuleFor(s => s.Id).NotEmpty();
        }
    }
}
