using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserLite
{
    public class GetApplicationUserLiteQueryValidator : AbstractValidator<GetApplicationUserLiteQuery>
    {
        public GetApplicationUserLiteQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
