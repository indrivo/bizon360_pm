using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.UserInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.UploadFile
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, HttpResponseMessage>
    {
        private readonly IStorageBaseService _service;

        private readonly IUserTokenDataService _tokenDataService;

        private readonly IUserAccessor _userAccessor;

        public UploadFileCommandHandler(IStorageBaseService service, IUserTokenDataService tokenDataService, IUserAccessor userAccessor)
        {
            _service = service;
            _tokenDataService = tokenDataService;
            _userAccessor = userAccessor;
        }

        public async Task<HttpResponseMessage> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _service.UploadFile(new CloudStorageApiRequestModel()
            {
                Path = request.Filepath,
                File = request.FormFile,
                AccessToken = await _tokenDataService.GetUserAccessToken(
                      Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value),
                      request.ExternalProvider)
            });
            return response;
        }
    }
}
