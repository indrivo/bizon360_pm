using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.DeleteSubComment
{
    internal class DeleteSubCommentCommandValidator : AbstractValidator<DeleteSubCommentCommand>
    {
        public DeleteSubCommentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
