using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Candidates.Commands.MoveCandidateToEmployee
{
    public class MoveCandidateToEmployeeCommandValidator : AbstractValidator<MoveCandidateToEmployeeCommand>
    {
        public MoveCandidateToEmployeeCommandValidator()
        {
            RuleFor(a => a.CandidateId)
                .NotNull()
                .NotEmpty().WithMessage("No candidate was matched to become an employee");
        }
    }
}
