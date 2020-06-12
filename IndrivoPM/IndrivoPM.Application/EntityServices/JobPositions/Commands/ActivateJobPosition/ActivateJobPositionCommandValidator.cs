using FluentValidation;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.ActivateJobPosition
{
   internal class ActivateJobPositionCommandValidator : AbstractValidator<ActivateJobPositionCommand>
    {
        public ActivateJobPositionCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull();
        }
    }
}
