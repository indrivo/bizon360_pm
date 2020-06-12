using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.BulkActions.BulkDeleteRequest
{
    public class BulkDeleteRequestCommandValidator : AbstractValidator<BulkDeleteRequestCommand>
    {
        public BulkDeleteRequestCommandValidator()
        {
            RuleFor(request => request.Requests).NotEmpty();
        }
    }
}
