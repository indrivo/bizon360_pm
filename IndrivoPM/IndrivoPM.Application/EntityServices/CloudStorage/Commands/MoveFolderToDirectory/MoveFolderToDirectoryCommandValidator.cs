﻿using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFolderToDirectory
{
    public class MoveFolderToDirectoryCommandValidator: AbstractValidator<MoveFolderToDirectoryCommand>
    {
        public MoveFolderToDirectoryCommandValidator()
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
