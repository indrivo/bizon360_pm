using Gear.CloudStorage.Abstractions.Domain;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFolderContentIntoAnotherDirectory
{
    public class MoveFolderContentIntoAnotherDirectoryCommand : IRequest
    {
        public string OldPath { get; set; }

        public string NewPath { get; set; }

        public string FolderName { get; set; }

        public int ProjectNumber { get; set; }

        public ExternalProviders ExternalProvider { get; set; }
    }
}
