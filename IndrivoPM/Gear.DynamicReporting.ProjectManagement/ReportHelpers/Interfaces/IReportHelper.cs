using System;
using System.Collections.Generic;
using Gear.Manager.Core.EntityServices.Reports.Enums;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetEmployeeFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetGeneralByFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetOverdueTasksFilteredReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectGroupsFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetProjectsByFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.Filters.GetTeamsByFiltersReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetActivityListByProjectReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetGeneralSprintListByProjectReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetLoggedTimeByPeriodList;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGeneralReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectGroupsGeneralReport;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime;

namespace Gear.DynamicReporting.ProjectManagement.ReportHelpers.Interfaces
{
    /// <summary>
    /// The interface for generic reports. Adding a new method for report, make sure that it returns the array of bytes
    /// (byte[]) as the result (having string as a returning type we may receive, for example, error that our .xls file is corrupt).
    /// If you want to implement filters for including/excluding columns of the table header, you should add "IList of TTableCell" tableHeaders
    /// (defining in the corresponding table helpers the default structure of the table)...
    /// </summary>
    public interface IReportHelper<TTableCell> where TTableCell : class
    {
        byte[] GetEmployeeLoggedTime(UserLoggedTimeListViewModel request, DateTime startDate, DateTime dueDate);

        byte[] GetEmployeesLoggedTimeByPeriod(LoggedTimeByPeriodListViewModel request, DateTime startDate, DateTime dueDate);

        byte[] GetProjectGeneralReport(ProjectGeneralReportListViewModel request, DateTime startDate, DateTime dueDate);

        byte[] GetProjectGroupsListReport(ProjectGroupsGeneralReportListViewModel request, DateTime startDate, DateTime dueDate);

        byte[] GetActivityListsByProjectReport(ActivityListByProjectReportListViewModel request);

        byte[] GetSprintListGeneralReport(SprintListGeneralReportListViewModel request, IList<TTableCell> tableHeaders);

        byte[] GetProjectsByFiltersReport(ProjectFilteredReportListViewModel request, IList<TTableCell> tableHeaders);

        byte[] GetProjectGroupsFilteredReportWithHistory(ProjectGroupFilteredReportListViewModel request,
            DateTime startDate, DateTime dueDate, Interval intervalType, IList<TTableCell> tableHeaders);

        byte[] GetTeamsByFiltersReport(TeamsFilteredReportListViewModel request, IList<TTableCell> tableHeaders);

        byte[] GetOverdueTasksFilteredReport(OverdueTasksFilteredReportViewModel request, IList<TTableCell> tableHeaders);
        
        byte[] GetEmployeeFilteredReport(EmployeeFilteredReportListViewModel request, IList<TTableCell> tableHeaders);

        byte[] GetGeneralFilteredReport(GeneralFilteredReportListViewModel request, IList<TTableCell> tableHeaders);
    }
}
