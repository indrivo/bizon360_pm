using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.UpdateChangeRequest
{
    public class UpdateChangeRequestCommandValidator : AbstractValidator<UpdateChangeRequestCommand>
    {
        public UpdateChangeRequestCommandValidator()
        {
            RuleFor(cr => cr.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(cr => cr.Description)
                .MaximumLength(1000);

            RuleFor(cr => cr.ProjectId)
                .NotNull();
        }
    }
}
