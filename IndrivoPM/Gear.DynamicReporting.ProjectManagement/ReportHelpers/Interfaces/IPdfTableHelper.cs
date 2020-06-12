using System.Collections.Generic;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Interfaces;

namespace Gear.DynamicReporting.ProjectManagement.ReportHelpers.Interfaces
{
    public interface IPdfTableHelper : IPdfTableHelperStyles
    {
        List<PdfTableCell> GetEmployeeLoggedTimeTableHeaders();

        List<PdfTableCell> GetEmployeesLoggedTimeByPeriodTableHeaders();

        List<PdfTableCell> GetProjectGeneralReportTableHeaders();

        List<PdfTableCell> GetProjectGroupsListReportTableHeaders();

        List<PdfTableCell> GetActivityListsByProjectReportTableHeaders(List<string> activityTypes);
    }
}
