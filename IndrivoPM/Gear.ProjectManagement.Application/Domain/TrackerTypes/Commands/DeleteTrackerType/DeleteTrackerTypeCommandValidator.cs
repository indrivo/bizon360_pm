using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.TrackerTypes.Commands.DeleteTrackerType
{
    public class DeleteTrackerTypeCommandValidator:AbstractValidator<DeleteTrackerTypeCommand>
    {
        public DeleteTrackerTypeCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull();
        }
    }
}
