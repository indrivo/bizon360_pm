using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.HrmEntities.Enums;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.BackgroundProcesses.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Background.LoggedTimePerDay
{
    public class LoggedTimeScopedService : IScopeBase<LoggedTimeScopedService>
    {
        private readonly IGearContext _context;

        private readonly IMediator _mediator;

        public LoggedTimeScopedService(IMediator mediator, IGearContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task ExecuteProcess()
        {
            var usersList = _context.ApplicationUsers.Where(x => x.Active).ToList();

            foreach (var user in usersList)
            {
                var hours = new int();
                switch (user.EmploymentType)
                {
                    case EmploymentType.PartTime4H:
                        hours = 4;
                        break;
                    case EmploymentType.PartTime6H:
                        hours = 6;
                        break;
                    case EmploymentType.FullTime8H:
                        hours = 8;
                        break;
                    //Future Proofing the service
                    default:
                        hours = 8;
                        break;
                }

                if (_context.LoggedTimes.Where(x =>
                        x.DateOfWork == DateTime.Now.Date
                        && x.UserId == user.Id).Sum(x => x.Time) < hours || _context.LoggedTimes.Any(x =>
                        x.DateOfWork == DateTime.Now.Date
                        && x.UserId == user.Id))
                {
                   
                }
                else
                {
                    usersList.Remove(user);
                }
            }

            await _mediator.Publish(new LoggedTimeNotification() { Emails = usersList.Select(x => x.Email).ToList()}, CancellationToken.None);
        }
    }
}
