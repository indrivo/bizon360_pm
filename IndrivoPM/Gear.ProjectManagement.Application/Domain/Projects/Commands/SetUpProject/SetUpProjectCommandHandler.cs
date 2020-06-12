using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.SetUpProject
{
    public class SetUpProjectCommandHandler : IRequestHandler<SetUpProjectCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public SetUpProjectCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(SetUpProjectCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.ProjectSettings.FirstAsync(x => x.ProjectId == request.ProjectId, cancellationToken: cancellationToken);

            entity.DailyEmailsLoggedTimeActivityType = request.DailyEmailsLoggedTimeActivityType;
            entity.WeeklyEmailsLoggedTimeActivityType = request.WeeklyEmailsLoggedTimeActivityType;
            entity.MonthlyEmailsLoggedTimeActivityType = request.MonthlyEmailsLoggedTimeActivityType;

            entity.DailyEmailsLoggedTimeSprint = request.DailyEmailsLoggedTimeSprint;
            entity.WeeklyEmailsLoggedTimeSprint = request.WeeklyEmailsLoggedTimeSprint;
            entity.MonthlyEmailsLoggedTimeSprint = request.MonthlyEmailsLoggedTimeSprint;

            entity.ProjectProjectTab = request.ProjectProjectTab;
            entity.ProjectActivitiesTab = request.ProjectActivitiesTab;
            entity.ProjectChangeRequestsTab = request.ProjectChangeRequestsTab;
            entity.ProjectLoggedTimeTab = request.ProjectLoggedTimeTab;
            entity.ProjectWikiAndFilesTab = request.ProjectWikiAndFilesTab;
            entity.ProjectReportsTab = request.ProjectReportsTab;
            entity.ProjectInvoiceTab = request.ProjectInvoiceTab;

            entity.ActivityActivityTab = request.ActivityActivityTab;
            entity.ActivityLinkedActivitiesTab = request.ActivityLinkedActivitiesTab;
            entity.ActivityLoggedTimeTab = request.ActivityLoggedTimeTab;
            entity.ActivityCommentsTab = request.ActivityCommentsTab;
            entity.ActivityHistoryTab = request.ActivityHistoryTab;

            entity.ProjectNotificationOnUpdateCompleteArchive = request.ProjectNotificationOnUpdateCompleteArchive;
            entity.ProjectNotificationWeeklyDeadline = request.ProjectNotificationWeeklyDeadline;
            entity.ProjectNotificationDailyDeadline = request.ProjectNotificationDailyDeadline;
            entity.ProjectNotificationDailyOverdue = request.ProjectNotificationDailyOverdue;

            entity.ActivityNotificationOnCreateUpdateCompleteDelete =
                request.ActivityNotificationOnCreateUpdateCompleteDelete;
            entity.ActivityNotificationWeeklyDeadline = request.ActivityNotificationWeeklyDeadline;
            entity.ActivityNotificationDailyDeadline = request.ActivityNotificationDailyDeadline;
            entity.ActivityNotificationDailyOverdue = request.ActivityNotificationDailyOverdue;

            entity.SprintNotificationOnCreateUpdateCompleteDelete =
                request.SprintNotificationOnCreateUpdateCompleteDelete;
            entity.SprintNotificationWeeklyDeadline = request.SprintNotificationWeeklyDeadline;
            entity.SprintNotificationDailyDeadline = request.SprintNotificationDailyDeadline;
            entity.SprintNotificationDailyOverdue = request.SprintNotificationDailyOverdue;

            entity.ActivityChangeName = request.ActivityChangeName;
            entity.ActivityChangeProject = request.ActivityChangeProject;
            entity.ActivityChangeStatus = request.ActivityChangeStatus;
            entity.ActivityChangeProirity = request.ActivityChangeProirity;
            entity.ActivityChangeTeam = request.ActivityChangeTeam;
            entity.ActivityChangeEstimatedTime = request.ActivityChangeEstimatedTime;
            entity.ActivityChangeActivityType = request.ActivityChangeActivityType;
            entity.ActivityChangeActivityList = request.ActivityChangeActivityList;
            entity.ActivityChangeSprint = request.ActivityChangeSprint;
            entity.ActivityChangeStartDueDate = request.ActivityChangeStartDueDate;
            entity.ActivityDelete = request.ActivityDelete;

            

            _context.ProjectSettings.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
