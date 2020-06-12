using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetNotificationProfileList
{
    public class GetNotificationProfileListQueryHandler : IRequestHandler<GetNotificationProfileListQuery, NotificationProfilesListViewModel>
    {
        private readonly INotificationProfileService _service;
        private readonly IGearContext _context;

        public GetNotificationProfileListQueryHandler(INotificationProfileService service, IGearContext context)
        {
            _service = service;
            _context = context;
        }

        public async Task<NotificationProfilesListViewModel> Handle(GetNotificationProfileListQuery request, CancellationToken cancellationToken)
        {
            var notificationsDto = await _service.GetNotificationProfilesList();

            var result = new List<NotificationProfileListLookupModel>();

            foreach (var item in notificationsDto)
            {
                var lookupModel = new NotificationProfileListLookupModel();
                var userDictionary = new Dictionary<Guid, string>();

                foreach (var u in item.Users)
                {
                    var user = _context.ApplicationUsers.Find(u);
                    var userName = user.FirstName + " " + user.LastName;

                    userDictionary.Add(u, userName);
                }

                lookupModel.Id = item.Id;
                lookupModel.Name = item.Name;
                lookupModel.UserList = userDictionary;

                result.Add(lookupModel);
            }

            return new NotificationProfilesListViewModel()
            {
                NotificationProfiles = result
            };
        }
    }
}
