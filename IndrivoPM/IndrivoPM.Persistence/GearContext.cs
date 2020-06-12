using Audit.Core;
using Audit.EntityFramework;
using Gear.Common.Entities;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain;
using Gear.Domain.AppEntities;
using Gear.Domain.HrmEntities;
using Gear.Domain.HrmEntities.Recruitment;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Settings;
using Gear.Domain.PmEntities.Wiki;
using Gear.Domain.ReportEntities;
using Gear.Domain.ReportEntities.FilterEntities;
using Gear.DynamicReporting.Config.Configurations;
using Gear.Hrm.Persistence.Config.Configurations;
using Gear.Identity.Permissions.Config.Configs;
using Gear.Identity.Permissions.Domain.Entities;
using Gear.Identity.Permissions.Services;
using Gear.Persistence.Settings;
using Gear.Pm.Persistence.Config.Configurations;
using Gear.Sstp.Abstractions.Domain;
using Gear.Sstp.Config.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Gear.Persistence
{
    public class GearContext : AuditIdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IGearContext, IPermissionContext
    {
        private readonly IUserAccessor _userAccessor;
        
        public GearContext(DbContextOptions<GearContext> options, IUserAccessor accessor, 
            IOptions<DatabaseConfigs> settings) : base(options)
        {
            _userAccessor = accessor;

            Audit.EntityFramework.Configuration.Setup()
                .ForContext<GearContext>(config => config
                    .IncludeEntityObjects()
                    .AuditEventType(settings.Value.DefaultConnection))
                .UseOptOut()
                .IgnoreAny(t => t.Name.EndsWith("History"))
                .Ignore<ApplicationUser>()
                .Ignore<ApplicationRole>()
                .Ignore<ApplicationUserRole>()
                .Ignore<UserDepartmentTeam>()
                .Ignore<JobDepartmentTeam>()
                .Ignore<ProjectMember>()
                .Ignore<ActivityAssignee>()
                .Ignore<ServiceType>()
                .Ignore<ProductType>()
                .Ignore<SolutionType>()
                .Ignore<TechnologyType>()
                .Ignore<IdentityUserToken<Guid>>()
                .Ignore<IdentityRoleClaim<Guid>>()
                .Ignore<Report<Guid>>()
                .Ignore<TableHeader<Guid>>()
                .Ignore<ReportTableHeader<Guid>>()
                .Ignore<UserReport<Guid>>()
                .Ignore<ReportFilter<Guid>>()
                .Ignore<UserGuidFilter<Guid>>()
                .Ignore<UserDateFilter<Guid>>()
                .Ignore<UserActivityStatusFilter<Guid>>()
                .Ignore<UserProjectStatusFilter<Guid>>()
                .Ignore<ProjectActivityType>();

            Audit.Core.Configuration.Setup()
                .UseEntityFramework(_ => _
                    .AuditTypeMapper(t => typeof(AuditLog))
                    .AuditEntityAction<AuditLog>((ev, entry, entity) =>
                    {
                        entity.EntityTypeName = entry.EntityType.Name;
                        entity.AuditDate = DateTime.Now;
                        entity.AuditUserName = Environment.UserName;
                        entity.EntityPk = entry.PrimaryKey.First().Value.ToString();
                        entity.TableName = entry.Table;
                        entity.Title = AuditEventType?.ToString() ?? "not setted";
                        entity.AuditUserId = GetCurrentUser();
                        entity.Action = entry.Action;
                        if (entry.Changes != null && entry.Changes.Any())
                        {
                            var diferences = new List<EntryChange>();

                            foreach (var eventEntryChange in entry.Changes)
                            {
                                EntryChange change = new EntryChange(
                                    columnName:eventEntryChange.ColumnName,
                                    originalValue: eventEntryChange.OriginalValue == null ? "empty" : eventEntryChange.OriginalValue.ToString(),
                                    newValue: eventEntryChange.NewValue == null ? "empty" : eventEntryChange.NewValue.ToString());

                                if (!change.OriginalValue.Equals(change.NewValue))
                                {
                                    diferences.Add(change);
                                }
                            }

                            var setUpChanges = entity.SetChanges(diferences);
                            if (setUpChanges.IsFailure)
                            {
                                //todo:add logger and log to the service.
                                Console.WriteLine(setUpChanges.Error);
                            }
                        }
                        
                    })
                    .IgnoreMatchedProperties());

            Database.SetCommandTimeout(19000);
        }

        public DbContext Instance => this;
        
        public DbSet<Report<Guid>> Reports { get; set; }
        
        public DbSet<ReportFilter<Guid>> ReportFilters { get; set; }
        
        public DbSet<ReportTableHeader<Guid>> ReportTableHeaders { get; set; }
        
        public DbSet<TableHeader<Guid>> TableHeaders { get; set; }
        
        public DbSet<UserReport<Guid>> UserReports { get; set; }
        
        public DbSet<UserActivityStatusFilter<Guid>> UserActivityStatusFilters { get; set; }
        
        public DbSet<UserDateFilter<Guid>> UserDateFilters { get; set; }
        
        public DbSet<UserGuidFilter<Guid>> UserGuidFilters { get; set; }

        public DbSet<UserProjectStatusFilter<Guid>> UserProjectStatusFilters { get; set; }

        public DbSet<ServiceTimeChecker> ServiceTimeCheckers { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Platform> Platforms { get; set; }

        public new DbSet<ApplicationUserRole> UserRoles { get; set; }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<BusinessUnit> BusinessUnits { get; set; }

        public DbSet<DepartmentTeam> DepartmentTeams { get; set; }

        public DbSet<UserDepartmentTeam> UserDepartmentTeams { get; set; }

        public DbSet<JobDepartmentTeam> JobDepartmentTeams { get; set; }

        public DbSet<ProjectGroup> ProjectGroups { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectMember> ProjectMembers { get; set; }

        public DbSet<ProjectSettings> ProjectSettings { get; set; }

        public DbSet<ProjectActivityType> ProjectActivityTypes { get; set; }

        public DbSet<ActivityList> ActivityLists { get; set; }

        public DbSet<ActivityAssignee> ActivityAssignees { get; set; }

        public DbSet<Sprint> Sprints { get; set; }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<JobPosition> JobPositions { get; set; }

        public DbSet<LoggedTime> LoggedTimes { get; set; }

        public DbSet<TrackerType> TrackerTypes { get; set; }

        public DbSet<CheckItem> CheckItems { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<EntryChange> EntryChanges { get; set; }

        public DbSet<IdentityUserToken<Guid>> AspNetUserTokens { get; set; }

        public DbSet<ServiceType> ServiceTypes { get; set; }

        public DbSet<SolutionType> SolutionTypes { get; set; }

        public DbSet<TechnologyType> TechnologyTypes { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        public DbSet<ChangeRequest> ChangeRequests { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<RecruitmentPipeline> RecruitingPipelines { get; set; }


        public DbSet<RecruitmentStage> RecruitmentStages { get; set; }

        public DbSet<RoleToPermissions> RolesToPermissions { get; set; }

        public DbSet<ModulesForUser> ModulesForUsers { get; set; }

        public DbSet<MainComment> MainComments { get; set; }

        public DbSet<SubComment> SubComments { get; set; }

        public DbSet<Headline> WikiHeadlines { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<ProjectInvoice> ProjectInvoices { get; set; }

        public DbSet<T> Set<T>() where T : BaseModel
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Override default save to save as well additional information for persistence.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        
        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                switch (entry.Entity)
                {
                    case BaseModel trackable:
                        {
                            var userId = GetCurrentUser();
                            switch (entry.State)
                            {
                                case EntityState.Modified:
                                    typeof(BaseModel).GetProperty("ModifiedBy")?.SetValue(trackable, userId, null);
                                    typeof(BaseModel).GetProperty("ModifyTime")?.SetValue(trackable, DateTime.Now, null);
                                    break;

                                case EntityState.Added:
                                    if (typeof(BaseModel).GetProperty("Id").GetValue(trackable).Equals(Guid.Empty))
                                    {
                                        typeof(BaseModel).GetProperty("Id")?.SetValue(trackable, Guid.NewGuid(), null);
                                    }
                                    typeof(BaseModel).GetProperty("CreatedBy")?.SetValue(trackable, userId, null);
                                    typeof(BaseModel).GetProperty("CreatedTime")?.SetValue(trackable, DateTime.Now, null);
                                    typeof(BaseModel).GetProperty("ModifiedBy")?.SetValue(trackable, userId, null);
                                    typeof(BaseModel).GetProperty("ModifyTime")?.SetValue(trackable, DateTime.Now, null);
                                    break;
                            }

                            break;
                        }
                }
            }
        }

        private Guid GetCurrentUser()
        {
            var user = _userAccessor.GetUser();
            return !user.Identity.IsAuthenticated ? Guid.Empty
                : Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GearContext).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectConfig).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ServiceTypeConfig).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DepartmentConfig).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModulesForUserConfig).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReportFilterConfig).Assembly);

            modelBuilder.HasSequence<int>("ReferenceSequenceProjects")
                .IncrementsBy(1);
            modelBuilder.HasSequence<int>("ReferenceSequenceActivities")
                .IncrementsBy(1);
            modelBuilder.HasSequence<int>("ReferenceSequenceActivityLists")
                .IncrementsBy(1);
            modelBuilder.HasSequence<int>("ReferenceSequenceChangeRequests")
                .IncrementsBy(1);

            base.OnModelCreating(modelBuilder);
        }

    }
}