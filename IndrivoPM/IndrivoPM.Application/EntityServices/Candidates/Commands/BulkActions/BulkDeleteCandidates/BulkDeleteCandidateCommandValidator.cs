using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.BulkActions.BulkDeleteCandidates
{
    public class BulkDeleteCandidateCommandValidator : AbstractValidator<BulkDeleteCandidateCommand>
    {
        public BulkDeleteCandidateCommandValidator()
        {
            RuleFor(p => p.Candidates).NotEmpty();
        }
    }
}
