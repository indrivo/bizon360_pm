using FluentValidation;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.UpdateOrderJobPosition
{
    public class UpdateOrderJobPositionCommandValidator : AbstractValidator<UpdateOrderJobPositionCommand>
    {
        public UpdateOrderJobPositionCommandValidator()
        {
            RuleFor(x => x.jobPositionIds).NotNull();
        }
    }
}
