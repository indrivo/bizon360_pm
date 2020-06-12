using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.CreateSection
{
    public class CreateSectionCommandValidator : AbstractValidator<CreateSectionCommand>
    {
        public CreateSectionCommandValidator()
        {
            RuleFor(s => s.Id).NotNull().NotEmpty();

            RuleFor(s => s.Title)
                .NotEmpty()
                .MaximumLength(256);

            RuleFor(s => s.HeadlineId).NotNull().NotEmpty();
        }
    }
}
