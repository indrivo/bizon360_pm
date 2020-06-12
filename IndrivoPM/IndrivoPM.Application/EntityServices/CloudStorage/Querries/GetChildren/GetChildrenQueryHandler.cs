using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.UserInjection;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Querries.GetChildren
{
    public class GetChildrenQueryHandler : IRequestHandler<GetChildrenQuery, List<CloudMetaData>>
    {
        private readonly IStorageBaseService _storageBaseService;

        private readonly IUserTokenDataService _userTokenDataService;

        private readonly IUserAccessor _userAccessor;

        public GetChildrenQueryHandler(IStorageBaseService storageBaseService, IUserTokenDataService userTokenDataService, IUserAccessor userAccessor)
        {
            _storageBaseService = storageBaseService;
            _userTokenDataService = userTokenDataService;
            _userAccessor = userAccessor;
        }


        public async Task<List<CloudMetaData>> Handle(GetChildrenQuery request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            var children = await _storageBaseService.GetChildren(new CloudStorageApiRequestModel()
            {
                Path = request.FilePath,
                AccessToken = await _userTokenDataService.GetUserAccessToken(
                    userId,
                    request.ExternalProvider)
            });

            return children;
        }
    }
}
