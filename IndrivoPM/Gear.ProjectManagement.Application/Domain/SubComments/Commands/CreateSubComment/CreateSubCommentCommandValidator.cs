using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.CreateSubComment
{
    internal class CreateSubCommentCommandValidator : AbstractValidator<CreateSubCommentCommand>
    {
        public CreateSubCommentCommandValidator()
        {
            RuleFor(x => x.MainCommentId).NotNull().NotEmpty();

            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
