using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.BulkActions.BulkRefuseRequest
{
    public class BulkRefuseRequestCommandValidator : AbstractValidator<BulkRefuseRequestCommand>
    {
        public BulkRefuseRequestCommandValidator()
        {
            RuleFor(request => request.Requests).NotEmpty();
        }
    }
}
