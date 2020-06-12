using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Service.Abstractions;
using Gear.Common.Extensions.UserInjection;
using MediatR;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Querries.CheckUserToken
{
    public class CheckUserTokenQueryHandler : IRequestHandler<CheckUserTokenQuery, bool>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IUserTokenDataService _tokenDataService;
        public CheckUserTokenQueryHandler(IUserAccessor userAccessor, IUserTokenDataService tokenDataService)
        {
            _userAccessor = userAccessor;
            _tokenDataService = tokenDataService;
        }
        public async Task<bool> Handle(CheckUserTokenQuery request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);
            return await _tokenDataService.CheckUserToken(userId);
        }
    }
}
