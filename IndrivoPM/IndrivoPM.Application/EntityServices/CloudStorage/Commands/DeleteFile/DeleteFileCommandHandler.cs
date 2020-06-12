using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.UserInjection;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Commands.DeleteFile
{
    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
    {
        private readonly IStorageBaseService _service;
        private readonly IUserTokenDataService _tokenDataService;
        private readonly IUserAccessor _userAccessor;
        public DeleteFileCommandHandler(IStorageBaseService service, IUserTokenDataService tokenDataService, IUserAccessor userAccessor)
        {
            _service = service;
            _tokenDataService = tokenDataService;
            _userAccessor = userAccessor;
        }
        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            await _service.DeleteElement(new CloudStorageApiRequestModel()
            {
                Id = request.Id,
                Path = request.FilePath,
                ElementName = request.FileName,
                AccessToken = await _tokenDataService.GetUserAccessToken(Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value), request.ExternalProvider)
            });
            return Unit.Value;
        }
    }
}
