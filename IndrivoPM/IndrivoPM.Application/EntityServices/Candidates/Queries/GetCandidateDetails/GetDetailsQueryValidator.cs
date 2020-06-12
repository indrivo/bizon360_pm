using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Queries.GetCandidateDetails
{
    public class GetDetailsQueryValidator : AbstractValidator<GetCandidateDetailsQuery>
    {
        public GetDetailsQueryValidator()
        {
            RuleFor(a => a.Id)
                .NotNull().NotEmpty();
        }
    }
}
