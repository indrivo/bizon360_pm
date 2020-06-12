using FluentValidation;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.DeleteJobPosition
{
    public class DeleteJobPositionCommandValidator:AbstractValidator<DeleteJobPositionCommand>
    {
        public DeleteJobPositionCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull();

        }
    }
}
