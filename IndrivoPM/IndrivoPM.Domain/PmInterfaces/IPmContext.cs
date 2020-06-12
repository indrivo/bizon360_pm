using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Settings;
using Gear.Domain.PmEntities.Wiki;
using Microsoft.EntityFrameworkCore;

namespace Gear.Domain.PmInterfaces
{
    public interface IPmContext
    {
        DbContext Instance { get; }

        DbSet<ProjectGroup> ProjectGroups { get; set; }

        DbSet<Project> Projects { get; set; }

        DbSet<ProjectMember> ProjectMembers { get; set; }

        DbSet<ProjectSettings> ProjectSettings { get; set; }

        DbSet<ProjectInvoice> ProjectInvoices { get; set; }

        DbSet<ProjectActivityType> ProjectActivityTypes { get; set; }

        DbSet<ActivityList> ActivityLists { get; set; }

        DbSet<ActivityAssignee> ActivityAssignees { get; set; }

        DbSet<Sprint> Sprints { get; set; }

        DbSet<Activity> Activities { get; set; }

        DbSet<ActivityType> ActivityTypes { get; set; }

        DbSet<LoggedTime> LoggedTimes { get; set; }

        DbSet<TrackerType> TrackerTypes { get; set; }

        DbSet<CheckItem> CheckItems { get; set; }

        DbSet<ChangeRequest> ChangeRequests { get; set; }

        DbSet<MainComment> MainComments { get; set; }

        DbSet<SubComment> SubComments { get; set; }

        DbSet<Headline> WikiHeadlines { get; set; }

        DbSet<Section> Sections { get; set; }
    }
}
