using FluentValidation;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.CreateJobPosition
{
   public class CreateJobPositionCommandValidator:AbstractValidator<CreateJobPositionCommand>
    {
        public CreateJobPositionCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.Description).MaximumLength(1000);

            RuleFor(x => x.Abbreviation).MaximumLength(20);
        }
    }
}
