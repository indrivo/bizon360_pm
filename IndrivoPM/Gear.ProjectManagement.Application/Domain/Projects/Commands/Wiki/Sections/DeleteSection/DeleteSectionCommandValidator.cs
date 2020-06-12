using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Sections.DeleteSection
{
    public class DeleteSectionCommandValidator : AbstractValidator<DeleteSectionCommand>
    {
        public DeleteSectionCommandValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
