using FluentValidation;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateJobPosition
{
    public class UpdateJobPositionCommandValidator:AbstractValidator<UpdateJobPositionCommand>
    {
        public UpdateJobPositionCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();

            RuleFor(x => x.Description).MaximumLength(1000);

            RuleFor(x => x.Abbreviation).MaximumLength(20);

        }
    }
}
