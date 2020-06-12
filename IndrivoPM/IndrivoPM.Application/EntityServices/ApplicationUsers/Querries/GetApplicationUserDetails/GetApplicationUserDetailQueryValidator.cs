using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserDetails
{
    public class GetApplicationUserDetailQueryValidator : AbstractValidator<GetApplicationUserDetailQuery>
    {
        public GetApplicationUserDetailQueryValidator()
        {
            RuleFor(u => u.Id).NotEmpty();
        }
    }
}
