using FluentValidation;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFolderContentIntoAnotherDirectory
{
    public class MoveFolderContentIntoAnotherDirectoryCommandValidator : AbstractValidator<MoveFolderContentIntoAnotherDirectoryCommand>
    {
        public MoveFolderContentIntoAnotherDirectoryCommandValidator()
        {
            RuleFor(x => x.FolderName)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.NewPath)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.OldPath)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.ProjectNumber)
                .GreaterThanOrEqualTo(0);
        }
    }
}
