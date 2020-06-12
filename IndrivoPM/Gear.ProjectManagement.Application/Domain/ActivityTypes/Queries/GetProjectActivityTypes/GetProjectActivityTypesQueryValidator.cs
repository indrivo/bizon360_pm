using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityTypes.Queries.GetProjectActivityTypes
{
    public class GetProjectActivityTypesQueryValidator : AbstractValidator<GetProjectActivityTypesQuery>
    {
        public GetProjectActivityTypesQueryValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
