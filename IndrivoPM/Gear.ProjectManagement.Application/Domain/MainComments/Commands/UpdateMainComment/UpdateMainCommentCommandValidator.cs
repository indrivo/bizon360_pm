using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.UpdateMainComment
{
    internal class UpdateMainCommentCommandValidator : AbstractValidator<UpdateMainCommentCommand>
    {
        public UpdateMainCommentCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();

            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
