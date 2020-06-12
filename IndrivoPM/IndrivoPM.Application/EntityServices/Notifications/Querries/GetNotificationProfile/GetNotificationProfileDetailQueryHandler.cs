using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using Gear.Notifications.Abstractions.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.Notifications.Querries.GetNotificationProfile
{
    public class GetNotificationProfileDetailQueryHandler : IRequestHandler<GetNotificationProfileDetailQuery, NotificationProfileDetailModel>
    {
        private readonly INotificationProfileService _notificationProfileService;

        private readonly IEventService _eventService;

        private readonly IGearContext _context;

        public GetNotificationProfileDetailQueryHandler(INotificationProfileService notificationProfileService, IGearContext context, IEventService eventService)
        {
            _notificationProfileService = notificationProfileService;
            _context = context;
            _eventService = eventService;
        }

        public async Task<NotificationProfileDetailModel> Handle(GetNotificationProfileDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _notificationProfileService.GetNotificationProfile(request.Id);

            var users = _context.ApplicationUsers.Include(x => x.JobPosition)
                .Where(x => entity.Users.Contains(x.Id));

            var allEventsInDb = await _eventService.GetEvents();

            var result = allEventsInDb.Where(x => entity.Events.Contains(x.Id)).ToList();

            return new NotificationProfileDetailModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                UserList = users.ToList(),
                EventList = result
            };
        }
    }
}
