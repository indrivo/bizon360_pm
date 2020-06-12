using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.DeleteChangeRequest
{
    public class DeleteChangeRequestCommandValidator : AbstractValidator<DeleteChangeRequestCommand>
    {
        public DeleteChangeRequestCommandValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
