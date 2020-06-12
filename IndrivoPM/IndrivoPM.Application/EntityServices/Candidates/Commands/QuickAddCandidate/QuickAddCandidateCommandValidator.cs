using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.QuickAddCandidate
{
    public class QuickAddCandidateCommandValidator : AbstractValidator<QuickAddCandidateCommand>
    {
        public QuickAddCandidateCommandValidator()
        {
            RuleFor(a => a.FirstName)
                .NotEmpty()
                .MaximumLength(75);

            RuleFor(a => a.LastName)
                .NotEmpty()
                .MaximumLength(75);

            RuleFor(a => a.Email)
                .NotEmpty()
                .MaximumLength(75)
                .EmailAddress();

            RuleFor(a => a.PhoneNumber)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(a => a.CandidateStatus)
                .NotNull();

            RuleFor(a => a.RecruitmentStageId)
                .NotNull()
                .NotEmpty();

            RuleFor(a => a.JobPositionId)
                .NotNull()
                .NotEmpty();

            RuleFor(a => a.DepartmentId)
                .NotEmpty();
        }
    }
}
