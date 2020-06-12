using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain.Models;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.UserInjection;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFileToFolder
{
    public class MoveFileToFolderCommandHandler : IRequestHandler<MoveFileToFolderCommand, Unit>
    {
        private readonly IStorageBaseService _service;

        private readonly IUserTokenDataService _tokenDataService;

        private readonly IUserAccessor _userAccessor;

        public MoveFileToFolderCommandHandler(IStorageBaseService service, IUserTokenDataService tokenDataService,
            IUserAccessor userAccessor)
        {
            _service = service;
            _tokenDataService = tokenDataService;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(MoveFileToFolderCommand request, CancellationToken cancellationToken)
        {
            await _service.MoveFileToFolder(new MoveFileRequestModel(request.NewParentFolderId, request.FullFilePath)
            {
                AccessToken = await _tokenDataService.GetUserAccessToken(Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value), request.ExternalProvider)
            });

            return Unit.Value;
        }
    }
}
