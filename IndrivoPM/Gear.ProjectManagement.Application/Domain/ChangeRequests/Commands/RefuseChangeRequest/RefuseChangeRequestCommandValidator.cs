using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.RefuseChangeRequest
{
    public class RefuseChangeRequestCommandValidator : AbstractValidator<RefuseChangeRequestCommand>
    {
        public RefuseChangeRequestCommandValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
