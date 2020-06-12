using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Commands.CreateChangeRequest
{
    public class CreateChangeRequestCommandValidator : AbstractValidator<CreateChangeRequestCommand>
    {
        public CreateChangeRequestCommandValidator()
        {
            RuleFor(cr => cr.Name)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(cr => cr.Description)
                .NotEmpty();
                

            RuleFor(cr => cr.ProjectId)
                .NotNull();
        }
    }
}
