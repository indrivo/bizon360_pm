using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityAudit
{
    public class GetActivityAuditQueryValidator : AbstractValidator<GetActivityAuditQuery>
    {
        public GetActivityAuditQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
