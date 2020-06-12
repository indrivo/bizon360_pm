using System.Collections.Generic;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;

namespace Gear.DynamicReporting.ProjectManagement.ReportHelpers.Interfaces
{
    public interface IExcelTableHelper
    {
        List<ExcelTableCell> GetEmployeeLoggedTimeTableHeaders();

        List<ExcelTableCell> GetEmployeesLoggedTimeByPeriodTableHeaders();

        List<ExcelTableCell> GetProjectGeneralReportTableHeaders();

        List<ExcelTableCell> GetProjectGroupsListReportTableHeaders();

        List<ExcelTableCell> GetActivityListsByProjectReportTableHeaders(List<string> activityTypes);

        List<ExcelTableCell> GetSprintListGeneralReportTableHeaders();

        List<ExcelTableCell> GetTeamsByFiltersReportTableHeaders();

        List<ExcelTableCell> GetProjectGroupsWithHistoryFilteredReportTableHeaders();

        List<ExcelTableCell> GetOverdueTasksFilteredReportTableHeaders();

        List<ExcelTableCell> GetGeneralFilteredReportTableHeaders();
    }
}
