using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.SubComments.Commands.UpdateSubComment
{
    internal class UpdateSubCommentCommandValidator : AbstractValidator<UpdateSubCommentCommand>
    {
        public UpdateSubCommentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();

            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
