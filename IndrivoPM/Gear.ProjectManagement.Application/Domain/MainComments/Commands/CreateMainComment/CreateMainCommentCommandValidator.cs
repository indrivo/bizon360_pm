using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Commands.CreateMainComment
{
    internal class CreateMainCommentCommandValidator : AbstractValidator<CreateMainCommentCommand>
    {
        public CreateMainCommentCommandValidator()
        {
            RuleFor(x => x.RecordId).NotNull().NotEmpty();

            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
