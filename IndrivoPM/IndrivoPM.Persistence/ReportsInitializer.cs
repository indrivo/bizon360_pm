using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Domain.ReportEntities;
using Gear.Domain.ReportEntities.Enums;
using Gear.Domain.ReportEntities.FilterEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gear.Persistence
{
    public class ReportsInitializer
    {
        public static async Task Initialize(GearContext context, UserManager<ApplicationUser> userManager)
        {
            var initializer = new ReportsInitializer();
            await initializer.Seed(context, userManager);
        }

        public async Task Seed(GearContext context, UserManager<ApplicationUser> userManager)
        {
            if (context != null)
            {
                await SeedReports(context);

                await SeedHeaders(context);

                await SeedReportHeader(context);

                await SeedUserReports(context);

                await SeedAllowedFilters(context);

                await SeedUserProjectStatusFilters(context);

                await SeedUserActivityStatusFilters(context);

                await SeedUserDateFilters(context);

                await SeedUserGuidFilters(context, userManager);

                await SeedReportNotificationServices(context);
            }
        }

        public static async Task SeedReports(GearContext context)
        {
            if (!await context.Reports.AnyAsync())
            {
                await context.Reports.AddRangeAsync(new List<Report<Guid>>
                {
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Employee logged time"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Employee logged time by period"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Project general"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Project group list"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Activity lists"}, 
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Sprint list"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Projects by filters"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Teams by filters"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Project groups by filters"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "General filtered"},
                    new Report<Guid> {Id = Guid.NewGuid(), Name = "Overdue"}
                });

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedHeaders(GearContext context)
        {
            if (!await context.TableHeaders.AnyAsync())
            {
                var tableHeaders = new List<TableHeader<Guid>>();
                
                // Sprint List
                tableHeaders.AddRange(new List<TableHeader<Guid>>
                {
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 10, Order = 1, Active = true, Deletable = false, Name = "#"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 15, Order = 2, Active = true, Deletable = true, Name = "ID"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 3, Active = true, Deletable = true, Name = "Activity name"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 4, Active = true, Deletable = true, Name = "Assignee"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 5, Active = true, Deletable = true, Name = "Status"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 6, Active = true, Deletable = false, Name = "Est. time (hours)"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 7, Active = true, Deletable = false, Name = "Log. time (hours)"}
                });

                // Overdue Report
                tableHeaders.AddRange(new List<TableHeader<Guid>>
                {
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 10, Order = 1, Active = true, Deletable = false, Name = "#"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 2, Active = true, Deletable = true, Name = "ProjectGroup"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 3, Active = true, Deletable = true, Name = "Project"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 4, Active = true, Deletable = true, Name = "Activity"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 5, Active = true, Deletable = true, Name = "Assignee"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 40, Order = 6, Active = true, Deletable = false, Name = "Deadline"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 30, Order = 7, Active = true, Deletable = false, Name = "Overdue (days)"}
                });

                // General filtered
                tableHeaders.AddRange(new List<TableHeader<Guid>>
                {
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 10, Order = 1, Active = true, Deletable = false, Name = "#"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 2, Active = true, Deletable = true, Name = "ProjectGroup"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 3, Active = true, Deletable = true, Name = "Project"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 4, Active = true, Deletable = true, Name = "Sprint"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 5, Active = true, Deletable = true, Name = "Activity"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 6, Active = true, Deletable = true, Name = "Assignee"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 7, Active = true, Deletable = false, Name = "Est. time (hours)"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 8, Active = true, Deletable = false, Name = "Log. time (hours)"}
                });

                // Project group (History)
                tableHeaders.AddRange(new List<TableHeader<Guid>>
                {
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 10, Order = 1, Active = true, Deletable = false, Name = "#"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 2, Active = true, Deletable = true, Name = "ProjectGroup"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 3, Active = true, Deletable = true, Name = "Project"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 4, Active = true, Deletable = true, Name = "Assignee"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 40, Order = 5, Active = true, Deletable = true, Name = "Period"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 40, Order = 6, Active = true, Deletable = true, Name = "Total Logged (hours)"},
                });

                // Teams by filters
                tableHeaders.AddRange(new List<TableHeader<Guid>>
                {
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 10, Order = 1, Active = true, Deletable = false, Name = "#"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 2, Active = true, Deletable = true, Name = "Assignee"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 3, Active = true, Deletable = true, Name = "Project"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 4, Active = true, Deletable = true, Name = "Activity"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 5, Active = true, Deletable = true, Name = "Activity status"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 7, Active = true, Deletable = false, Name = "Est. time (hours)"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 8, Active = true, Deletable = false, Name = "Log. time (hours)"}
                });

                // Projects by filters
                tableHeaders.AddRange(new List<TableHeader<Guid>>
                {
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 10, Order = 1, Active = true, Deletable = false, Name = "#"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 2, Active = true, Deletable = true, Name = "Project"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 3, Active = true, Deletable = true, Name = "Assignee"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 60, Order = 4, Active = true, Deletable = true, Name = "Activity name"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 5, Active = true, Deletable = true, Name = "Activity status"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 5, Active = true, Deletable = true, Name = "Activity type"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 7, Active = true, Deletable = false, Name = "Est. time (hours)"},
                    new TableHeader<Guid> {Id = Guid.NewGuid(), Width = 20, Order = 8, Active = true, Deletable = false, Name = "Log. time (hours)"}
                });

                await context.TableHeaders.AddRangeAsync(tableHeaders);

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedReportHeader(GearContext context)
        {
            if (!await context.ReportTableHeaders.AnyAsync())
            {
                var reports = await context.Reports.ToListAsync();

                var employees = await context.ApplicationUsers.ToListAsync();

                var tableHeaders = await context.TableHeaders.ToListAsync();

                var reportTableHeaders = (from report in reports
                from employee in employees
                from header in tableHeaders
                select new ReportTableHeader<Guid>
                {
                    UserId = employee.Id, ReportId = report.Id, TableHeaderId = header.Id, Active = true,
                }).ToList();

                await context.ReportTableHeaders.AddRangeAsync(reportTableHeaders);

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUserReports(GearContext context)
        {
            if (!await context.UserReports.AnyAsync())
            {
                var employees = await context.ApplicationUsers.ToListAsync();

                var reports = await context.Reports.ToListAsync();

                var userReports = 
                    (from employee in employees 
                    from report in reports 
                    select new UserReport<Guid> {ReportId = report.Id, UserId = employee.Id}).ToList();

                await context.AddRangeAsync(userReports);

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedAllowedFilters(GearContext context)
        {
            if (!await context.ReportFilters.AnyAsync())
            {
                var employees = await context.ApplicationUsers.ToListAsync();

                var reports = await context.Reports.ToListAsync();

                var filters = new List<FilterType>
                {
                    FilterType.Guid,
                    FilterType.StartDate,
                    FilterType.DueDate,
                    FilterType.ActivityStatus,
                    FilterType.ProjectStatus,
                    FilterType.ProjectGroupIds,
                    FilterType.ProjectIds,
                    FilterType.ActivityListIds,
                    FilterType.SprintIds,
                    FilterType.ActivityIds,
                    FilterType.ActivityTypeIds,
                    FilterType.EmployeeIds,
                };

                var reportFilters = 
                    (from employee in employees 
                    from report in reports 
                    from filterType in filters.Distinct() 
                    select new ReportFilter<Guid>
                    {
                        Active = true, 
                        ReportId = report.Id, 
                        UserId = employee.Id, 
                        FilterType = filterType
                    })
                    .ToList();

                await context.ReportFilters.AddRangeAsync(reportFilters.Distinct());

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUserProjectStatusFilters(GearContext context)
        {
            if (!await context.UserProjectStatusFilters.AnyAsync())
            {
                var projectStatuses = new List<ProjectStatus>
                {
                    ProjectStatus.New,
                    ProjectStatus.InProgress,
                    ProjectStatus.OnHold,
                    ProjectStatus.Canceled,
                    ProjectStatus.Completed
                };

                var reports = await context.Reports.ToListAsync();

                var users = await context.ApplicationUsers.ToListAsync();

                var projectStatusFilters = 
                    (from report in reports 
                    from user in users 
                    from projectStatus in projectStatuses 
                    select new UserProjectStatusFilter<Guid> 
                        {Id = Guid.NewGuid(), UserId = user.Id, ReportId = report.Id, ProjectStatus = projectStatus})
                    .ToList();

                await context.UserProjectStatusFilters.AddRangeAsync(projectStatusFilters);

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUserActivityStatusFilters(GearContext context)
        {
            if (!await context.UserActivityStatusFilters.AnyAsync())
            {
                var activityStatuses = new List<ActivityStatus>
                {
                    ActivityStatus.New,
                    ActivityStatus.InProgress,
                    ActivityStatus.Refused,
                    ActivityStatus.Developed,
                    ActivityStatus.Tested,
                    ActivityStatus.Completed
                };

                var employees = await context.ApplicationUsers.ToListAsync();

                var reports = await context.Reports.ToListAsync();

                var activityStatusFilters = 
                    (from employee in employees 
                    from report in reports 
                    from activityStatus in activityStatuses 
                    select new UserActivityStatusFilter<Guid> 
                        {Id = Guid.NewGuid(), ReportId = report.Id, UserId = employee.Id, ActivityStatus = activityStatus})
                    .ToList();

                await context.UserActivityStatusFilters.AddRangeAsync(activityStatusFilters);

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUserDateFilters(GearContext context)
        {
            // The Due date currently is omitted intentionally.
            if (!await context.UserDateFilters.AnyAsync())
            {
                var employees = await context.ApplicationUsers.ToListAsync();
                
                var reports = await context.Reports.ToListAsync();

                var userDateFilters = 
                    (from employe in employees
                    from report in reports
                    select new UserDateFilter<Guid>
                    {
                        Id = Guid.NewGuid(),
                        UserId = employe.Id,
                        FilterType = FilterType.StartDate,
                        ReportId = report.Id,
                        Date = DateTime.Now.AddYears(-1)
                    }).ToList();

                await context.UserDateFilters.AddRangeAsync(userDateFilters);
                
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedUserGuidFilters(GearContext context, UserManager<ApplicationUser> userManager)
        {
            if (!await context.UserGuidFilters.AnyAsync())
            {
                var projectGroupIds = await context.ProjectGroups.AnyAsync() 
                    ? await context.ProjectGroups.Select(x => x.Id).ToListAsync() 
                    : new List<Guid>();

                var projectIds = await context.Projects.AnyAsync() 
                    ? await context.Projects.Select(x => x.Id).ToListAsync() 
                    : new List<Guid>();

                var activityTypeIds = await context.ActivityTypes.AnyAsync() 
                    ? await context.ActivityTypes.Select(x => x.Id).ToListAsync() 
                    : new List<Guid>();

                var userIds = await context.ApplicationUsers
                    .Select(x => x.Id).ToListAsync();

                var owners = await userManager.GetUsersInRoleAsync("PM Owner");
                var admins = await userManager.GetUsersInRoleAsync("PM Admin");
                var employees = owners.Union(admins);

                var employeeIds = employees
                    .Select(x => x.Id).ToList();

                var reportIds = await context.Reports.AnyAsync() ?
                    await context.Reports
                        .Where(x => x.Name == "Overdue" 
                                    || x.Name == "Projects by filters"
                                    || x.Name == "teams by filters")
                        .Select(x => x.Id).ToListAsync() 
                    : new List<Guid>();

                var guidFilters = new List<UserGuidFilter<Guid>>();
                foreach(var reportId in reportIds)
                {
                    foreach (var userId in employeeIds)
                    {
                        guidFilters.AddRange(projectGroupIds.Select(projectGroupId => new UserGuidFilter<Guid>
                        {
                            Id = Guid.NewGuid(), EntityId = projectGroupId, UserId = userId, ReportId = reportId, FilterType = FilterType.ProjectGroupIds
                        }));

                        guidFilters.AddRange(projectIds.Select(projectId => new UserGuidFilter<Guid>
                        {
                            Id = Guid.NewGuid(), EntityId = projectId, UserId = userId, ReportId = reportId, FilterType = FilterType.ProjectIds
                        }));

                        guidFilters.AddRange(activityTypeIds.Select(activityTypeId => new UserGuidFilter<Guid>
                        {
                            Id = Guid.NewGuid(), EntityId = activityTypeId, UserId = userId, ReportId = reportId, FilterType = FilterType.ActivityTypeIds
                        }));

                        guidFilters.AddRange(userIds.Select(employeeId => new UserGuidFilter<Guid>
                        {
                            Id = Guid.NewGuid(), EntityId = employeeId, UserId = userId, ReportId = reportId, FilterType = FilterType.EmployeeIds
                        }));
                    }
                }

                await context.UserGuidFilters.AddRangeAsync(guidFilters);

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedReportNotificationServices(GearContext context)
        {
            if (!await context.ServiceTimeCheckers.AnyAsync())
            {
                await context.ServiceTimeCheckers.AddRangeAsync(new List<ServiceTimeChecker>
                {
                    new ServiceTimeChecker
                    {
                        ServiceId = Guid.NewGuid(), ServiceName = "OverdueAdm", ExecutedLastTime = null
                    },
                    new ServiceTimeChecker
                    {
                        ServiceId = Guid.NewGuid(), ServiceName = "OverduePm", ExecutedLastTime = null
                    },
                    new ServiceTimeChecker
                    {
                        ServiceId = Guid.NewGuid(), ServiceName = "DailyLogsAdm", ExecutedLastTime = null
                    },
                    new ServiceTimeChecker
                    {
                        ServiceId = Guid.NewGuid(), ServiceName = "DailyLogsPm", ExecutedLastTime = null
                    },
                    new ServiceTimeChecker
                    {
                        ServiceId = Guid.NewGuid(), ServiceName = "YesterdayLogsAdm", ExecutedLastTime = null
                    },
                    new ServiceTimeChecker
                    {
                        ServiceId = Guid.NewGuid(), ServiceName = "YesterdayLogsPm", ExecutedLastTime = null
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
