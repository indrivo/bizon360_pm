using System.Net.Http;
using Gear.CloudStorage.Abstractions.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<HttpResponseMessage>
    {
        public string Filepath { get; set; }

        public IFormFile FormFile { get; set; }

        public ExternalProviders ExternalProvider { get; set; }

    }
}
