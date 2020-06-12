using FluentValidation;

namespace Gear.Manager.Core.EntityServices.JobPositions.Queries.GetJobPositionDetails
{
    public class GetJobPositionDetailQueryValidator:AbstractValidator<GetJobPositionDetailQuery>
    {
        public GetJobPositionDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
