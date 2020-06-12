using FluentValidation;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFileToFolder
{
    class MoveFileToFolderCommandValidator : AbstractValidator<MoveFileToFolderCommand>
    {
        public MoveFileToFolderCommandValidator()
        {
            RuleFor(x => x.FullFilePath)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.NewParentFolderId)
                .NotEmpty()
                .NotNull();
        }
    }
}
