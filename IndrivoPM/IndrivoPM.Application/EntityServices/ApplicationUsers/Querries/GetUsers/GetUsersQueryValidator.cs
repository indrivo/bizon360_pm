using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetUsers
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(request => request.UsersIds).NotNull().NotEmpty();
        }
    }
}
