using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.Wiki.Headlines.DeleteHeadline
{
    public class DeleteHeadlineCommandValidator : AbstractValidator<DeleteHeadlineCommand>
    {
        public DeleteHeadlineCommandValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
