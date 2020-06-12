using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidateStatus
{
    public class UpdateCandidateStatusCommandValidator : AbstractValidator<UpdateCandidateStatusCommand>
    {
        public UpdateCandidateStatusCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty().WithMessage("Candidate's Id was missed");

            RuleFor(a => a.CandidateStatus)
                .IsInEnum().WithMessage("Invalid candidate's status has been passed via parameter");
        }
    }
}
