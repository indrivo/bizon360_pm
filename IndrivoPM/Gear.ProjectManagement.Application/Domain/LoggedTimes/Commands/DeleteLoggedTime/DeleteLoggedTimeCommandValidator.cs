using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.LoggedTimes.Commands.DeleteLoggedTime
{
    public class DeleteLoggedTimeCommandValidator:AbstractValidator<DeleteLoggedTimeCommand>
    {
        public DeleteLoggedTimeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
