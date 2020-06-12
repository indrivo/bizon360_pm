using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.CreateHeadline
{
    public class CreateHeadlineCommandValidator : AbstractValidator<CreateHeadlineCommand>
    {
        public CreateHeadlineCommandValidator()
        {
            RuleFor(h => h.Title).NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.SectionName).NotEmpty()
                .MaximumLength(256);

            RuleFor(x => x.SectionBody).NotNull().NotEmpty();

            RuleFor(h => h.ProjectId).NotNull().NotEmpty();
        }
    }
}
