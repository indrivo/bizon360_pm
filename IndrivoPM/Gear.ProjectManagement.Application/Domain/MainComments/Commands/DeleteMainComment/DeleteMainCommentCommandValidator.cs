using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.DeleteMainComment
{
    internal class DeleteMainCommentCommandValidator : AbstractValidator<DeleteMainCommentCommand>
    {
        public DeleteMainCommentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
