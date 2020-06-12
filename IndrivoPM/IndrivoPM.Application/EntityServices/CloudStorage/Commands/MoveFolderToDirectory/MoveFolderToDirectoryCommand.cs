using System;
using System.Collections.Generic;
using System.Text;
using Gear.CloudStorage.Abstractions.Domain;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFolderToDirectory
{
    public class MoveFolderToDirectoryCommand : IRequest
    {
        public string OldPath { get; set; }
        public string NewPath { get; set; }
        public string FolderName { get; set; }
        public ExternalProviders ExternalProvider { get; set; }
        public int ProjectNumber { get; set; }
    }
}
