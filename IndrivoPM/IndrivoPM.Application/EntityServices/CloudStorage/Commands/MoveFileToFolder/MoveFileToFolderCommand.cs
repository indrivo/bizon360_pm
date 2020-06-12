using Gear.CloudStorage.Abstractions.Domain;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFileToFolder
{
    public class MoveFileToFolderCommand : IRequest
    {
        public string FullFilePath { get; set; }

        public string NewParentFolderId { get; set; }

        public ExternalProviders ExternalProvider { get; set; }
    }
}
