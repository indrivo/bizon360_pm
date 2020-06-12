using FluentValidation;

namespace Gear.Manager.Core.EntityServices.ApplicationUsers.Commands.DeleteApplicationUser
{
    public class DeleteApplicationUserCommandValidator:AbstractValidator<DeleteApplicationUserCommand>
    {
        public DeleteApplicationUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
