using FluentValidation;

namespace Gear.Identity.Manager.Domain.Roles.Queries.GetRoleDetails
{
    public class GetRoleDetailQueryValidator : AbstractValidator<GetRoleDetailQuery>
    {
        public GetRoleDetailQueryValidator()
        {
            RuleFor(x => x.RoleName)
                .NotNull()
                .NotEmpty();
        }
    }
}
