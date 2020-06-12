using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Domain.Models;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.UserInjection;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFolderContentIntoAnotherDirectory;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFolderToDirectory
{
    public class MoveFolderToDirectoryCommandHandler : IRequestHandler<MoveFolderToDirectoryCommand, Unit>
    {
        private readonly IStorageBaseService _service;

        private readonly IUserTokenDataService _tokenDataService;

        private readonly IUserAccessor _userAccessor;

        public MoveFolderToDirectoryCommandHandler(IStorageBaseService service,
            IUserTokenDataService tokenDataService,
            IUserAccessor userAccessor)
        {
            _service = service;
            _tokenDataService = tokenDataService;
            _userAccessor = userAccessor;
        }


        public async Task<Unit> Handle(MoveFolderToDirectoryCommand request, CancellationToken cancellationToken)
        {
            await _service.MoveFolderIntoAnotherDirectory(
                new MoveFolderContentFolderRequestModel(request.OldPath, request.NewPath, request.FolderName, request.ProjectNumber)
                {
                    AccessToken = await _tokenDataService.GetUserAccessToken(
                        Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value),
                        request.ExternalProvider)
                });

            return Unit.Value;
        }
    }

}
