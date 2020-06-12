using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.DeleteCandidate
{
    public class DeleteCandidateCommandValidator : AbstractValidator<DeleteCandidateCommand>
    {
        public DeleteCandidateCommandValidator()
        {
            RuleFor(a => a.CandidateId)
                .NotEmpty()
                .NotNull().WithMessage("Candidate's Id was missed");
        }
    }
}
