using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.UpdateHeadline
{
    public class UpdateHeadlineCommandValidator : AbstractValidator<UpdateHeadlineCommand>
    {
        public UpdateHeadlineCommandValidator()
        {
            RuleFor(h => h.Id).NotNull().NotEmpty();

            RuleFor(h => h.Title)
                .NotEmpty()
                .MaximumLength(256);
        }
    }
}
