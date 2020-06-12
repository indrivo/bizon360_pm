using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.UpdateSection
{
    public class UpdateSectionCommandValidator : AbstractValidator<UpdateSectionCommand>
    {
        public UpdateSectionCommandValidator()
        {
            RuleFor(s => s.Id).NotNull().NotEmpty();

            RuleFor(s => s.Title)
                .NotEmpty()
                .MaximumLength(256);
        }
    }
}
