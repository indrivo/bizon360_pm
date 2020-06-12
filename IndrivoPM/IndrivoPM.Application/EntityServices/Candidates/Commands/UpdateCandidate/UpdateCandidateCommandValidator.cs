using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.UpdateCandidate
{
    public class UpdateCandidateCommandValidator : AbstractValidator<UpdateCandidateCommand>
    {
        public UpdateCandidateCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty().WithMessage("Candidate's Id was missed");

            RuleFor(a => a.FirstName)
                .NotEmpty().WithMessage("The first name is required")
                .MaximumLength(75).WithMessage("Candidate's first name cannot exceed 75 characters.");

            RuleFor(a => a.LastName)
                .NotEmpty().WithMessage("The last name is required")
                .MaximumLength(75).WithMessage("Candidate's last name cannot exceed 75 characters.");

            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("The email is required")
                .MaximumLength(75).WithMessage("Candidate's email cannot exceed 75 characters.")
                .EmailAddress();

            RuleFor(a => a.PhoneNumber)
                .NotEmpty().WithMessage("The first name is required")
                .MaximumLength(25).WithMessage("Candidate's phone number cannot exceed 25 characters.");
        }
    }
}
