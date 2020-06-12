using Gear.CloudStorage.Abstractions.Domain;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.DeleteFile
{
    public class DeleteFileCommand : IRequest
    {
        public string Id { get; set; }
        public string FilePath { get; set; }

        public string AlternativeFileName { get; set; }

        public string FileName { get; set; }

        public ExternalProviders ExternalProvider { get; set; }
    }
}
