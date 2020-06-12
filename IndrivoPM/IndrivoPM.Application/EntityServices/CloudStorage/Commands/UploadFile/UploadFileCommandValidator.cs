using FluentValidation;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.UploadFile
{
    public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidator()
        {
            RuleFor(x => x.Filepath)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.FormFile)
                .NotNull();

            RuleFor(x => x.FormFile)
                .Must(x => x.Length <= 60 * 1024 * 1024);

            RuleFor(a => a.FormFile.Name)
                .Matches(@"^[^\\\/\:\*\?\<\>\|""]+$")
                .WithMessage(@"Name should not contain special characters: '\', '/', ':', '*', '?', '<', '>' and "" symbol. ");

            RuleFor(x => x.ExternalProvider)
                .NotNull();
        }
    }
}
