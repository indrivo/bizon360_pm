using FluentValidation;

namespace Gear.Manager.Core.EntityServices.JobPositions.Commands.RenameJobPosition
{
    public class RenameJobPositionCommandValidator : AbstractValidator<RenameJobPositionCommand>
    {
        public RenameJobPositionCommandValidator()
        {
            RuleFor(x => x.Id).NotNull();

            RuleFor(x => x.Name).MaximumLength(256)
                .NotEmpty();
        }
    }
}
