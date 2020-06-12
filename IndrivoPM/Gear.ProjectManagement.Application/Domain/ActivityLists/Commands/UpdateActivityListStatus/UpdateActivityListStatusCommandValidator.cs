using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ActivityLists.Commands.UpdateActivityListStatus
{
    public class UpdateActivityListStatusCommandValidator : AbstractValidator<UpdateActivityListStatusCommand>
    {
        public UpdateActivityListStatusCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(a => a.ActivityListStatus)
                .NotNull();
        }
    }
}
