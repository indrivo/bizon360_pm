using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.DeleteApplicationUsers
{
    public class DeleteApplicationUsersCommandValidator: AbstractValidator<DeleteApplicationUsersCommand>
    {
        public DeleteApplicationUsersCommandValidator()
        {
            RuleFor(x => x.UsersById).NotEmpty();
        }
    }
}
