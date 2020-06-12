using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkUpdateStatus
{
    public class BulkUpdateCandidateStatusCommandValidator : AbstractValidator<BulkUpdateCandidateStatusCommand>
    {
        public BulkUpdateCandidateStatusCommandValidator()
        {
            RuleFor(request => request.Candidates).NotEmpty();

            RuleFor(request => request.Status).IsInEnum();
        }
    }
}
