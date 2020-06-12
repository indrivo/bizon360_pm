using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gear.DynamicReporting.Abstractions.Extensions;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.Base;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.ExcelTableHelpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Interfaces;
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
using Gear.Manager.Core.EntityServices.Reports.Queries.GetProjectLoggedTimeReportList;
using Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers
{
    /// <summary>
    /// Returns .xls dynamic reports. Each drawn worksheet should be returned as a sequence of the bytes i.e. byte[] 
    /// (if we will return it as a STRING from controller, the file will be corrupt). 
    /// The documentation could be found here: https://github.com/JanKallman/EPPlus/wiki
    /// The concrete example of ExcelPackage class: https://documentation.help/OfficeOpenXML/aeaf6091-72c2-93a1-bdcd-2c95e236dd1f.htm
    /// </summary>
    public class ExcelReportHelper : BaseExcelReportHelper, IReportHelper<ExcelTableCell>
    {
        #region Private Methods
        /// <summary>
        /// The repeated part for general reports.
        /// </summary>
        /// <param name="request">The corresponding request class.</param>
        /// <param name="startDate">The starting date.</param>
        /// <param name="dueDate">The ending date.</param>
        /// <returns></returns>
        private byte[] XmlForGeneralReport(LoggedTimeByPeriodListViewModel request, DateTime startDate, DateTime dueDate)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2, new List<int> { 10, 60, 40, 40 });

                worksheet.Cells["C2"].Value = "Estimated / logged time per employees report";

                SetXmlTopBorderStyle(ref worksheet, 3, 'B', 'F', ExcelBorderStyle.Thin);

                worksheet.Cells["C3"].Value = "Start date";
                worksheet.Cells["D3"].Value = $"{startDate:dddd, dd MMMM yyyy}";

                worksheet.Cells["C4"].Value = "Due date";
                worksheet.Cells["D4"].Value = $"{dueDate:dddd, dd MMMM yyyy}";

                SetXmlBottomBorderStyle(ref worksheet, 4, 'B', 'F', ExcelBorderStyle.Thin);

                worksheet.Cells["C3:C4"].Style.Font.Bold = true;

                worksheet.Cells["B7:E7"].Style.Font.Bold = true;

                SetXmlColumnCenterHorizontalAlignment(ref worksheet, new List<int> { 2 });
                SetXmlColumnLeftHorizontalAlignment(ref worksheet, new List<int> { 3 });
                SetXmlColumnRightHorizontalAlignment(ref worksheet, new List<int> { 4, 5 });

                var startRow = 7;
                var row = 7;

                SetXmlTableHeader(ref worksheet, startRow, 'B', new List<string> { "#", "Employee", "Est.", "Log." });

                SetXmlBottomBorderStyle(ref worksheet, startRow, 'B', 'F', ExcelBorderStyle.Thin);

                foreach (var loggedTime in request.UsersLoggedEstimatedTime)
                {
                    ++row;

                    var bColumn = $"B{row}";
                    worksheet.Cells[bColumn].Value = row - startRow;

                    var cColumn = $"C{row}";
                    worksheet.Cells[cColumn].Value = loggedTime.Name;

                    var dColumn = $"D{row}";
                    worksheet.Cells[dColumn].Value = loggedTime.UserEstimatedTimeByPeriod;
                    worksheet.Cells[dColumn].Style.Numberformat.Format = "0.00";

                    var eColumn = $"E{row}";
                    worksheet.Cells[eColumn].Value = loggedTime.UserLoggedTimeByPeriod;
                    worksheet.Cells[eColumn].Style.Numberformat.Format = "0.00";
                }

                SetXmlBottomBorderStyle(ref worksheet, row, 'B', 'F', ExcelBorderStyle.Thin);

                worksheet.Cells[$"C{row + 1}"].Value = "Total time";

                worksheet.Cells[$"D{row + 1}"].Value = request.TotalEstimatedTime;
                worksheet.Cells[$"D{row + 1}"].Style.Numberformat.Format = "0.00";

                worksheet.Cells[$"E{row + 1}"].Value = request.TotalLoggedTime;
                worksheet.Cells[$"E{row + 1}"].Style.Numberformat.Format = "0.00";

                worksheet.Cells["D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells["D4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                return package.GetAsByteArray();
            }
        }

        /// <summary>
        /// Sets the start and due dates for groups general reports. Extracted apart for code reuse.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="startDate">The starting date.</param>
        /// <param name="dueDate">The ending date.</param>
        private void XmlForProjectGroupsGeneralReport(ref ExcelWorksheet worksheet, DateTime startDate, DateTime dueDate)
        {
            SetXmlColumnsWidth(ref worksheet, 7, new List<int> { 30, 40 });

            worksheet.Cells["G3"].Value = "Start date";
            worksheet.Cells["G3"].Style.Font.Bold = true;
            worksheet.Cells["H3"].Value = $"{startDate:dddd, dd MMMM yyyy}";

            worksheet.Cells["G4"].Value = "Due date";
            worksheet.Cells["G4"].Style.Font.Bold = true;
            worksheet.Cells["H4"].Value = $"{dueDate:dddd, dd MMMM yyyy}";
        }

        /// <summary>
        /// Applies template changes on worksheet and is dedicated for Project Group Report.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="startRow">The row where we start to apply the template. In the 
        /// end it will indicate where does the drawn template finishes.</param>
        /// <param name="request">The class which contains the data for template.</param>
        private void SetProjectGroupTemplateXml(ref ExcelWorksheet worksheet, ref int startRow, ProjectGroupsGeneralReportLookupModel request)
        {
            SetXmlColumnsWidth(ref worksheet, 2, new List<int> { 10, 60, 10, 10 });

            worksheet.Cells[$"C{startRow}"].Value = $"Project Group: {request.ProjectGroupName}";

            var row = startRow + 1;

            SetXmlTopBorderStyle(ref worksheet, row, 'B', 'F', ExcelBorderStyle.Thin);

            worksheet.Cells[string.Format("B{0}:E{0}", row)].Style.Font.Bold = true;

            SetXmlColumnLeftHorizontalAlignment(ref worksheet, new List<int> { 3 });
            SetXmlColumnCenterHorizontalAlignment(ref worksheet, new List<int> { 2 });
            SetXmlColumnRightHorizontalAlignment(ref worksheet, new List<int> { 4, 5 });

            SetXmlTableHeader(ref worksheet, row, 'B', new List<string> { "#", "Project", "Est.", "Log." });

            SetXmlBottomBorderStyle(ref worksheet, row, 'B', 'F', ExcelBorderStyle.Thin);

            foreach (var project in request.Projects)
            {
                ++row;

                var bColumn = $"B{row}";
                worksheet.Cells[bColumn].Value = row - startRow;

                var cColumn = $"C{row}";
                worksheet.Cells[cColumn].Value = project.ProjectName;

                var dColumn = $"D{row}";
                worksheet.Cells[dColumn].Value = project.EstimatedTime;
                worksheet.Cells[dColumn].Style.Numberformat.Format = "0.00";

                var eColumn = $"E{row}";
                worksheet.Cells[eColumn].Value = project.LoggedTime;
                worksheet.Cells[eColumn].Style.Numberformat.Format = "0.00";
            }

            SetXmlBottomBorderStyle(ref worksheet, row, 'B', 'F', ExcelBorderStyle.Thin);

            worksheet.Cells[$"C{row + 1}"].Value = "Total time";

            worksheet.Cells[$"D{row + 1}"].Value = request.TotalEstimatedTime;
            worksheet.Cells[$"D{row + 1}"].Style.Numberformat.Format = "0.00";

            worksheet.Cells[$"E{row + 1}"].Value = request.TotalLoggedTime;
            worksheet.Cells[$"E{row + 1}"].Style.Numberformat.Format = "0.00";

            startRow = row + 1;
        }

        /// <summary>
        /// Applies template changes on worksheet and is dedicated for list of projects Report.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="startRow">The row where we start to apply the template. In the 
        /// end it will indicate where does the drawn template finishes.</param>
        /// <param name="request">The class which contains the data for template.</param>
        private void SetProjectListTemplateXml(ref ExcelWorksheet worksheet, ref int startRow, ProjectLoggedTimeReportListViewModel request)
        {
            SetXmlColumnsWidth(ref worksheet, 2, new List<int> { 10, 60, 10, 10 });

            // Write header for table.
            SetXmlTableHeader(ref worksheet, startRow, 'B', new List<string> { "#", "Project", "Est.", "Log." });
            SetXmlBottomBorderStyle(ref worksheet, startRow, 'B', 'F', ExcelBorderStyle.Thin);
            SetXmlTopBorderStyle(ref worksheet, startRow, 'B', 'F', ExcelBorderStyle.Thin);
            // Set styles for header
            worksheet.Cells[string.Format("B{0}:E{0}", startRow)].Style.Font.Bold = true;

            var row = startRow + 1;

            SetXmlColumnLeftHorizontalAlignment(ref worksheet, new List<int> { 3 });
            SetXmlColumnCenterHorizontalAlignment(ref worksheet, new List<int> { 2 });
            SetXmlColumnRightHorizontalAlignment(ref worksheet, new List<int> { 4, 5 });

            // Iteration for write data.
            foreach (var project in request.ProjectReports)
            {
                worksheet.Cells[$"B{row}"].Value = row - startRow;

                worksheet.Cells[$"C{row}"].Value = project.ProjectName;

                worksheet.Cells[$"D{row}"].Value = project.EstimatedTime;
                worksheet.Cells[$"D{row}"].Style.Numberformat.Format = "0.00";

                worksheet.Cells[$"E{row}"].Value = project.LoggedTime;
                worksheet.Cells[$"E{row}"].Style.Numberformat.Format = "0.00";

                ++row;
            }

            // Draw footer project list report.
            SetProjectListFooter(ref worksheet, row, request.TotalEstimatedTime, request.TotalEstimatedTime);

            startRow = row + 1;
        }

        /// <summary>
        /// Draws the footer (last part) of the report.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row where the changes start to be applied.</param>
        /// <param name="sumEstimatedTime">The total number of estimated time.</param>
        /// <param name="sumLoggedTime">The total number of logged time.</param>
        private void SetProjectListFooter(ref ExcelWorksheet worksheet, int row, float sumEstimatedTime, float sumLoggedTime)
        {
            SetXmlBottomBorderStyle(ref worksheet, row, 'B', 'F', ExcelBorderStyle.Thin);

            worksheet.Cells[$"C{row + 1}"].Value = "Total time";

            worksheet.Cells[$"D{row + 1}"].Value = sumEstimatedTime;
            worksheet.Cells[$"D{row + 1}"].Style.Numberformat.Format = "0.00";

            worksheet.Cells[$"E{row + 1}"].Value = sumLoggedTime;
            worksheet.Cells[$"E{row + 1}"].Style.Numberformat.Format = "0.00";
        }
        #endregion

        public byte[] GetEmployeeLoggedTime(UserLoggedTimeListViewModel request, DateTime startDate, DateTime dueDate)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2, new List<int> { 10, 60, 40, 30, 10, 30 });

                worksheet.Cells["C2"].Value = "Logged time per employee report";

                SetXmlTopBorderStyle(ref worksheet, 3, 'B', 'H', ExcelBorderStyle.Thin);

                SetXmlBottomBorderStyle(ref worksheet, 5, 'B', 'H', ExcelBorderStyle.Thin);

                worksheet.Cells["C3"].Value = "Employee";
                worksheet.Cells["D3"].Value = request.UserName;

                worksheet.Cells["C4"].Value = "Start date";
                worksheet.Cells["D4"].Value = $"{startDate:dddd, dd MMMM yyyy}";

                worksheet.Cells["C5"].Value = "Due date";
                worksheet.Cells["D5"].Value = $"{dueDate:dddd, dd MMMM yyyy}";

                SetXmlBottomBorderStyle(ref worksheet, 8, 'B', 'H', ExcelBorderStyle.Thin);

                worksheet.Cells["C3:C5"].Style.Font.Bold = true;

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, new List<int> { 3, 4, 5 });
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, new List<int> { 2 });
                SetXmlColumnRightHorizontalAlignment(ref worksheet, new List<int> { 6, 7 });

                var row = 8;
                SetXmlTableHeader(ref worksheet, row, 'B', new List<string> { "#", "Activity", "Project", "Subtype", "Log.", "Date" });
                worksheet.Cells[string.Format("B{0}:G{0}", row)].Style.Font.Bold = true;

                foreach (var loggedTime in request.LoggedTimes)
                {
                    ++row;

                    var bColumn = $"B{row}";
                    worksheet.Cells[bColumn].Value = row - 8;

                    var cColumn = $"C{row}";
                    worksheet.Cells[cColumn].Value = loggedTime.ActivityName;

                    var dColumn = $"D{row}";
                    worksheet.Cells[dColumn].Value = loggedTime.ProjectName;

                    var eColumn = $"E{row}";
                    worksheet.Cells[eColumn].Value = loggedTime.TrackerName;

                    var fColumn = $"F{row}";
                    worksheet.Cells[fColumn].Value = loggedTime.Time;
                    worksheet.Cells[fColumn].Style.Numberformat.Format = "0.00";

                    var gColumn = $"G{row}";
                    worksheet.Cells[gColumn].Value = $"{loggedTime.DateOfWork:dddd, dd MMMM yyyy}";
                }

                SetXmlBottomBorderStyle(ref worksheet, row, 'B', 'H', ExcelBorderStyle.Thin);

                worksheet.Cells[$"C{row + 1}"].Value = "Total logged time";
                worksheet.Cells[$"F{row + 1}"].Value = request.TotalLoggedTime;
                worksheet.Cells[$"F{row + 1}"].Style.Numberformat.Format = "0.00";

                SetXmlTopBorderStyle(ref worksheet, row + 4, 'B', 'H', ExcelBorderStyle.Thin);

                worksheet.Cells[$"C{row + 4}"].Value = "Estimated time (total)";
                worksheet.Cells[$"C{row + 4}"].Style.Font.Bold = true;

                worksheet.Cells[$"F{row + 4}"].Value = request.TotalEstimatedTime;
                worksheet.Cells[$"F{row + 4}"].Style.Font.Bold = true;
                worksheet.Cells[$"F{row + 4}"].Style.Numberformat.Format = "0.00";

                worksheet.Cells[$"C{row + 5}"].Value = "Logged time (total)";
                worksheet.Cells[$"C{row + 5}"].Style.Font.Bold = true;

                worksheet.Cells[$"F{row + 5}"].Value = request.TotalLoggedTime;
                worksheet.Cells[$"F{row + 5}"].Style.Font.Bold = true;
                worksheet.Cells[$"F{row + 5}"].Style.Numberformat.Format = "0.00";

                SetXmlBottomBorderStyle(ref worksheet, row + 5, 'B', 'H', ExcelBorderStyle.Thin);

                return package.GetAsByteArray();
            }
        }

        public byte[] GetEmployeesLoggedTimeByPeriod(LoggedTimeByPeriodListViewModel request, DateTime startDate, DateTime dueDate)
        {
            return XmlForGeneralReport(request, startDate, dueDate);
        }

        public byte[] GetProjectGeneralReport(ProjectGeneralReportListViewModel request, DateTime startDate, DateTime dueDate)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2, new List<int> { 10, 60, 40, 40, 40, 20, 10, 10, 15, 20, 40 });

                worksheet.Cells["C2"].Value = "Project General Report";

                SetXmlTopBorderStyle(ref worksheet, 3, 'B', 'M', ExcelBorderStyle.Thin);

                worksheet.Cells["C3"].Value = "Project";
                worksheet.Cells["D3"].Value = request.ProjectName;

                worksheet.Cells["C4"].Value = "Start date";
                worksheet.Cells["D4"].Value = $"{startDate:dddd, dd MMMM yyyy}";

                worksheet.Cells["C5"].Value = "Due date";
                worksheet.Cells["D5"].Value = $"{dueDate:dddd, dd MMMM yyyy}";

                SetXmlBottomBorderStyle(ref worksheet, 5, 'B', 'M', ExcelBorderStyle.Thin);

                worksheet.Cells["C3:C5"].Style.Font.Bold = true;

                worksheet.Cells["B8:L8"].Style.Font.Bold = true;

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, new List<int> { 3, 4, 5, 6, 7 });
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, new List<int> { 2, 11 });
                SetXmlColumnRightHorizontalAlignment(ref worksheet, new List<int> { 8, 9, 10, 12 });

                var startRow = 8;
                var row = 8;

                SetXmlTableHeader(ref worksheet, row, 'B', new List<string> { "#", "Activity List", "Sprint",
                    "Employee", "Activity", "Priority", "Est.", "Log.", "Progress", "Status", "Modified" });

                SetXmlBottomBorderStyle(ref worksheet, startRow, 'B', 'M', ExcelBorderStyle.Thin);

                foreach (var activity in request.Activities)
                {
                    ++row;

                    var bColumn = $"B{row}";
                    worksheet.Cells[bColumn].Value = row - startRow;

                    var cColumn = $"C{row}";
                    worksheet.Cells[cColumn].Value = activity.ActivityListName;

                    var dColumn = $"D{row}";
                    worksheet.Cells[dColumn].Value = activity.SprintName;

                    var eColumn = $"E{row}";
                    worksheet.Cells[eColumn].Value = activity.EmployeeName;

                    var fColumn = $"F{row}";
                    worksheet.Cells[fColumn].Value = activity.ActivityName;

                    var gColumn = $"G{row}";
                    worksheet.Cells[gColumn].Value = activity.ActivityPriority;

                    var hColumn = $"H{row}";
                    worksheet.Cells[hColumn].Value = activity.Estimated;
                    worksheet.Cells[hColumn].Style.Numberformat.Format = "0.00";

                    var iColumn = $"I{row}";
                    worksheet.Cells[iColumn].Value = activity.Logged;
                    worksheet.Cells[iColumn].Style.Numberformat.Format = "0.00";

                    var jColumn = $"J{row}";
                    worksheet.Cells[jColumn].Value = activity.Progress;

                    var kColumn = $"K{row}";
                    worksheet.Cells[kColumn].Value = activity.ActivityStatus;

                    var lColumn = $"L{row}";
                    worksheet.Cells[lColumn].Value = $"{activity.LastModified:dddd, dd MMMM yyyy}";
                }

                using (ExcelRange autoFilterCells = worksheet.Cells[$"C8:E{row}"])
                {
                    autoFilterCells.AutoFilter = true;
                }

                SetXmlBottomBorderStyle(ref worksheet, row, 'B', 'M', ExcelBorderStyle.Thin);

                worksheet.Cells[$"G{row + 1}"].Value = "Total time";

                worksheet.Cells[$"H{row + 1}"].Value = request.TotalEstimatedTime;
                worksheet.Cells[$"H{row + 1}"].Style.Numberformat.Format = "0.00";

                worksheet.Cells[$"I{row + 1}"].Value = request.TotalLoggedTime;
                worksheet.Cells[$"I{row + 1}"].Style.Numberformat.Format = "0.00";

                return package.GetAsByteArray();
            }
        }

        public byte[] GetProjectGroupsReport(ProjectGroupsGeneralReportListViewModel request, DateTime startDate, DateTime dueDate)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                XmlForProjectGroupsGeneralReport(ref worksheet, startDate, dueDate);

                var startRow = 6;
                foreach (var projectGroup in request.ProjectGroups)
                {
                    SetProjectGroupTemplateXml(ref worksheet, ref startRow, projectGroup);
                    startRow += 3;
                }

                return package.GetAsByteArray();
            }
        }

        public byte[] GetProjectGroupsListReport(ProjectGroupsGeneralReportListViewModel request, DateTime startDate, DateTime dueDate)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                XmlForProjectGroupsGeneralReport(ref worksheet, startDate, dueDate);

                var startRow = 6;
                foreach (var projectGroup in request.ProjectGroups)
                {
                    SetProjectGroupTemplateXml(ref worksheet, ref startRow, projectGroup);
                    startRow += 3;
                }

                return package.GetAsByteArray();
            }
        }

        public byte[] GetProjectListReport(ProjectLoggedTimeReportListViewModel request, DateTime startDate, DateTime dueDate)
        {
            using (var package = new ExcelPackage())
            {
                // Initialize worksheet.
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                // Write in worksheet start date and end date.
                XmlForProjectGroupsGeneralReport(ref worksheet, startDate, dueDate);

                var startRow = 6;
                SetProjectListTemplateXml(ref worksheet, ref startRow, request);

                return package.GetAsByteArray();
            }
        }

        public byte[] GetActivityListsByProjectReport(ActivityListByProjectReportListViewModel request)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                var columnWidths = new List<int> { 10, 60 };
                for (var i = 0; i <= request.ActivityTypes.Count; i++)
                {
                    columnWidths.Add(10);
                }
                columnWidths.AddRange(new List<int> { 30, 30, 30, 40 });

                SetXmlColumnsWidth(ref worksheet, 2, columnWidths);

                worksheet.Cells["C2"].Value = $"Project: {request.ProjectName}";

                worksheet.Cells[3, 2, 6 + request.ActivityTypes.Count, 2].Style.Font.Bold = true;

                var columnsForCenterAlign = new List<int> { 2 };
                for (var i = 0; i <= 1 + request.ActivityTypes.Count; i++)
                {
                    columnsForCenterAlign.Add(4 + i);
                }
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, columnsForCenterAlign);

                var columnsForLeftAlign = new List<int> { 3, 8 + request.ActivityTypes.Count };
                SetXmlColumnLeftHorizontalAlignment(ref worksheet, columnsForLeftAlign);

                var columnsForRightAlign = new List<int> { 6 + request.ActivityTypes.Count, 7 + request.ActivityTypes.Count };
                SetXmlColumnRightHorizontalAlignment(ref worksheet, columnsForRightAlign);

                var startRow = 3;
                var row = 3;

                var tableHeader = new List<string> { "#", "Activity list name" };
                foreach (var activityType in request.ActivityTypes)
                {
                    tableHeader.Add($"{activityType} %");
                }
                tableHeader.AddRange(new List<string> { "Avg. %", "Status", "Planned date", "Actual date", "Comment" });
                SetXmlTableHeader(ref worksheet, startRow, 'B', tableHeader);

                SetXmlColumnsBold(ref worksheet, startRow, 'B', tableHeader.Count);

                SetXmlBottomBorderStyle(ref worksheet, startRow, 'B', tableHeader.Count + 1, ExcelBorderStyle.Thin);

                char restOfTheColumns = 'B';
                foreach (var activityList in request.ActivityList)
                {
                    ++row;

                    var bColumn = $"B{row}";
                    worksheet.Cells[bColumn].Value = row - startRow;

                    var cColumn = $"C{row}";
                    worksheet.Cells[cColumn].Value = activityList.ActivityListName;

                    var column = 'D';
                    foreach (var activityType in activityList.ActivityTypes)
                    {
                        var pattern = $"{column}{row}";
                        if (activityType.Progress.HasValue)
                        {
                            worksheet.Cells[pattern].Value = activityType.Progress.Value;
                        }
                        else
                        {
                            worksheet.Cells[pattern].Value = "-";
                        }
                        worksheet.Cells[pattern].Style.Numberformat.Format = "0\\%";
                        ++column;
                    }

                    var previousColumn = --column;
                    var currentColumn = ++column;

                    worksheet.Cells[$"{currentColumn}{row}"].Calculate();
                    worksheet.Cells[$"{currentColumn}{row}"].Style.Numberformat.Format = "0\\%";

                    worksheet.Cells[$"{currentColumn}{row}"].Formula =
                        $"IF(COUNT(D{row}:{previousColumn}{row})=0,\"-\",AVERAGE(D{row}:{previousColumn}{row}))";

                    previousColumn = currentColumn;
                    column = ++currentColumn;

                    var formula = string.Format("IF(OR({0}{1}=0,{0}{1}=\"-\"),\"New\",IF({0}{1}=100,\"Completed\",\"Ongoing\"))", previousColumn, row);
                    worksheet.Cells[$"{currentColumn}{row}"].Calculate();
                    worksheet.Cells[$"{currentColumn}{row}"].Formula = formula;

                    if (activityList.PlannedDate.HasValue)
                    {
                        worksheet.Cells[$"{++column}{row}"].Value =
                            $"{activityList.PlannedDate.Value:dddd, dd MMMM yyyy}";
                    }
                    else
                    {
                        worksheet.Cells[$"{++column}{row}"].Value = "-";
                    }

                    worksheet.Cells[$"{++column}{row}"].Value = activityList.ActualDate.HasValue
                        ? $"{activityList.ActualDate.Value:dddd, dd MMMM yyyy}"
                        : "-";

                    restOfTheColumns = column;
                }

                SetXmlBottomBorderStyle(ref worksheet, row, 'B', ++restOfTheColumns, ExcelBorderStyle.Thin);

                return package.GetAsByteArray();
            }
        }

        public byte[] GetSprintListGeneralReport(SprintListGeneralReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2, 
                    tableHeaders.Where(x => x.IsActive).Select(x => x.Width).ToList());

                worksheet.Cells["C2"].Value = "Project";
                worksheet.Cells["D2"].Value = request.ProjectName;

                worksheet.Cells["C2:C2"].Style.Font.Bold = true;

                var leftAlignmentColumns = new List<int>();
                var rightAlignmentColumns = new List<int>();
                var centerAlignmentColumns = new List<int>();

                for(var i = 0; i < tableHeaders.Count; )
                {
                    switch (tableHeaders[i++].Alignment)
                    {
                        case ExcelHorizontalAlignment.Left:
                            leftAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Right:
                            rightAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Center:
                            centerAlignmentColumns.Add(i);
                            break;
                    }
                }

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, leftAlignmentColumns);
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, centerAlignmentColumns);
                SetXmlColumnRightHorizontalAlignment(ref worksheet, rightAlignmentColumns);

                var startRow = 4;
                var row = 4;

                var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
                var tableHelper = new ExcelTableHelper();
                var headers = tableHelper.GetSprintListGeneralReportTableHeaders();

                var estimatedTimeColumnNumber = 0;
                var loggedTimeColumnNumber = 0;

                foreach (var sprint in request.Sprints)
                {
                    var cColumn = $"C{row}";
                    worksheet.Cells[cColumn].Value = "Sprint:";
                    var dColumn = $"D{row}";
                    worksheet.Cells[dColumn].Value = sprint.SprintName;

                    cColumn = $"C{row + 1}";
                    worksheet.Cells[cColumn].Value = "Start Date:";
                    dColumn = $"D{row + 1}";
                    worksheet.Cells[dColumn].Value = $"{sprint.StartDate:dddd, dd MMMM yyyy}";

                    cColumn = $"C{row + 2}";
                    worksheet.Cells[cColumn].Value = "Due Date:";
                    dColumn = $"D{row + 2}";
                    worksheet.Cells[dColumn].Value = $"{sprint.DueDate:dddd, dd MMMM yyyy}";

                    row += 3;
                    startRow = ++row;

                    SetXmlTableHeader(ref worksheet, row, 'B', tableNames);
                    SetXmlColumnsBold(ref worksheet, row, 'B', tableNames.Count);
                    SetXmlBottomBorderStyle(ref worksheet, startRow, 'B', tableNames.Count, ExcelBorderStyle.Thin);

                    var startColumn = 'B';
                    
                    foreach (var activity in sprint.Activities)
                    {
                        ++row;
                        var currentColumn = startColumn;

                        foreach (var name in tableNames)
                        {
                            var pattern = $"{currentColumn++}{row}";

                            if (name == headers[0].Name)
                            {
                                worksheet.Cells[pattern].Value = row - startRow;
                            }
                            else if (name == headers[1].Name)
                            {
                                worksheet.Cells[pattern].Value = string.Format($"A{activity.ActivityNumber:00000}");
                                worksheet.Cells[pattern].Style.Numberformat.Format = "0";
                            }
                            else if (name == headers[2].Name)
                            {
                                worksheet.Cells[pattern].Value = activity.ActivityName;
                            }
                            else if (name == headers[3].Name)
                            {
                                worksheet.Cells[pattern].Value = activity.Assignees;
                            }
                            else if (name == headers[4].Name)
                            {
                                worksheet.Cells[pattern].Value = activity.ActivityStatus;
                            }
                            else if (name == headers[5].Name)
                            {
                                estimatedTimeColumnNumber = currentColumn;

                                worksheet.Cells[pattern].Value = activity.EstimatedTime;
                                worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                            }
                            else if (name == headers[6].Name)
                            {
                                loggedTimeColumnNumber = currentColumn;

                                worksheet.Cells[pattern].Value = activity.LoggedTime;
                                worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                            }
                        }
                    }
                    SetXmlBottomBorderStyle(ref worksheet, row, 'B', tableNames.Count, ExcelBorderStyle.Thin);

                    if (estimatedTimeColumnNumber > 0 || loggedTimeColumnNumber > 0)
                    {
                        if (estimatedTimeColumnNumber > 0)
                        {
                            worksheet.Cells[++row, estimatedTimeColumnNumber - 'A' - 1].Value = "Total:";

                            worksheet.Cells[row, estimatedTimeColumnNumber - 'A'].Value = sprint.TotalEstimatedTime;
                            worksheet.Cells[row, estimatedTimeColumnNumber - 'A'].Style.Numberformat.Format = "0.00";
                            
                            SetXmlColumnsBold(ref worksheet, row, (char)(estimatedTimeColumnNumber - 2), 3);
                        }
                        else
                        {
                            worksheet.Cells[++row, loggedTimeColumnNumber - 'A' - 1].Value = "Total:";

                            SetXmlColumnsBold(ref worksheet, row, (char)(loggedTimeColumnNumber - 2), 2);
                        }

                        if (loggedTimeColumnNumber > 0)
                        {
                            worksheet.Cells[row, loggedTimeColumnNumber - 'A'].Value = sprint.TotalLoggedTime;
                            worksheet.Cells[row, loggedTimeColumnNumber - 'A'].Style.Numberformat.Format = "0.00";
                        }
                    }

                    row += 3;
                    startRow = row;
                }

                //
                if (estimatedTimeColumnNumber > 0 || loggedTimeColumnNumber > 0)
                {
                    if (estimatedTimeColumnNumber > 0)
                    {
                        worksheet.Cells[startRow, estimatedTimeColumnNumber - 'A' - 1].Value = "All sprints total:";

                        worksheet.Cells[startRow, estimatedTimeColumnNumber - 'A'].Value = request.TotalEstimatedTime;
                        worksheet.Cells[startRow, estimatedTimeColumnNumber - 'A'].Style.Numberformat.Format = "0.00";

                        SetXmlColumnsBold(ref worksheet, row, (char)(estimatedTimeColumnNumber - 2), 3);
                    }
                    else
                    {
                        worksheet.Cells[startRow, loggedTimeColumnNumber - 'A' - 1].Value = "All sprints total:";

                        SetXmlColumnsBold(ref worksheet, row, (char)(loggedTimeColumnNumber - 2), 2);
                    }

                    if (loggedTimeColumnNumber > 0)
                    {
                        worksheet.Cells[startRow, loggedTimeColumnNumber - 'A'].Value = request.TotalLoggedTime;
                        worksheet.Cells[startRow, loggedTimeColumnNumber - 'A'].Style.Numberformat.Format = "0.00";
                    }
                }

                return package.GetAsByteArray();
            }
        }

        public byte[] GetProjectsByFiltersReport(ProjectFilteredReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2,
                    tableHeaders.Where(x => x.IsActive).Select(x => x.Width).ToList());

                worksheet.Cells["C2"].Value = "Projects report";
                worksheet.Cells["D2"].Value = $"{DateTime.Now:dddd, dd MMMM yyyy}";

                worksheet.Cells["C2:C2"].Style.Font.Bold = true;

                var leftAlignmentColumns = new List<int>();
                var rightAlignmentColumns = new List<int>();
                var centerAlignmentColumns = new List<int>();

                for (var i = 0; i < tableHeaders.Count;)
                {
                    switch (tableHeaders[i++].Alignment)
                    {
                        case ExcelHorizontalAlignment.Left:
                            leftAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Right:
                            rightAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Center:
                            centerAlignmentColumns.Add(i);
                            break;
                    }
                }

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, leftAlignmentColumns);
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, centerAlignmentColumns);
                SetXmlColumnRightHorizontalAlignment(ref worksheet, rightAlignmentColumns);

                var startRow = 4;
                var row = 4;

                var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
                var tableHelper = new ExcelTableHelper();
                var headers = tableHelper.GetProjectsByFiltersReportTableHeaders();

                var startColumn = 'B';

                SetXmlTableHeader(ref worksheet, row, startColumn, tableNames);
                SetXmlColumnsBold(ref worksheet, row, startColumn, tableNames.Count);
                SetXmlBottomBorderStyle(ref worksheet, startRow, startColumn, tableNames.Count, ExcelBorderStyle.Thin);

                var estimatedHoursColumn = 'A';
                var loggedHoursColumn = 'A';

                foreach (var project in request.Projects)
                {
                    var isProjectSetted = false;
                    foreach (var activity in project.ActivityView.Activities)
                    {
                        foreach (var assignee in activity.AssigneeView.Assignees)
                        {
                            var activityStatus = assignee.LoggedTimeView.LoggedTimes[0].ActivityStatus;
                            var activityType = assignee.LoggedTimeView.LoggedTimes[0].ActivityType;
                            
                            ++row;
                            var currentColumn = startColumn;

                            foreach (var name in tableNames)
                            {
                                var pattern = $"{currentColumn++}{row}";

                                if (name == headers[0].Name)
                                {
                                    worksheet.Cells[pattern].Value = row - startRow;
                                }
                                else if (name == headers[1].Name)
                                {
                                    if (isProjectSetted) continue;
                                    worksheet.Cells[pattern].Value = project.ProjectName;
                                    isProjectSetted = true;
                                }
                                else if (name == headers[2].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.ActivityName;
                                }
                                else if (name == headers[3].Name)
                                {
                                    worksheet.Cells[pattern].Value = assignee.AssigneeName;
                                }
                                else if (name == headers[4].Name)
                                {
                                    worksheet.Cells[pattern].Value = activityStatus;
                                }
                                else if (name == headers[5].Name)
                                {
                                    worksheet.Cells[pattern].Value = activityType;
                                }
                                else if (name == headers[6].Name)
                                {
                                    worksheet.Cells[pattern].Value = assignee.LoggedTimeView.TotalEstimatedTime;
                                    worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                    estimatedHoursColumn = currentColumn;
                                }
                                else if (name == headers[7].Name)
                                {
                                    worksheet.Cells[pattern].Value = assignee.LoggedTimeView.TotalLoggedTime;
                                    worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                    loggedHoursColumn = currentColumn;
                                }
                            }
                        }
                    }

                    var rowForTotal = row + 1;

                    if (estimatedHoursColumn != 'A' && loggedHoursColumn != 'A')
                    {
                        worksheet.Cells[$"A{rowForTotal}"].Value = "Total:";
                        worksheet.Cells[$"A{rowForTotal}"].Style.Font.Bold = true;

                        if (estimatedHoursColumn != 'A')
                        {
                            worksheet.Cells[$"{--estimatedHoursColumn}{rowForTotal}"].Value =
                                project.ActivityView.TotalEstimatedTime;
                            worksheet.Cells[$"{estimatedHoursColumn}{rowForTotal}"].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[$"{estimatedHoursColumn}{rowForTotal}"].Style.Font.Bold = true;
                        }

                        if (loggedHoursColumn != 'A')
                        {
                            worksheet.Cells[$"{--loggedHoursColumn}{rowForTotal}"].Value =
                                project.ActivityView.TotalLoggedTime;
                            worksheet.Cells[$"{loggedHoursColumn}{rowForTotal}"].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[$"{loggedHoursColumn}{rowForTotal}"].Style.Font.Bold = true;
                        }
                    }

                    startRow = row = rowForTotal + 1;
                }

                var rowForFinal = row + 1;

                if (estimatedHoursColumn == 'A' || loggedHoursColumn == 'A') return package.GetAsByteArray();
                worksheet.Cells[$"A{rowForFinal}"].Value = "Total:";
                worksheet.Cells[$"A{rowForFinal}"].Style.Font.Bold = true;

                if (estimatedHoursColumn != 'A')
                {
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Value = request.TotalEstimatedTime;
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Style.Font.Bold = true;
                }

                if (loggedHoursColumn == 'A') return package.GetAsByteArray();
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Value = request.TotalLoggedTime;
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Style.Numberformat.Format = "0.00";
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Style.Font.Bold = true;

                return package.GetAsByteArray();
            }
        }

        public byte[] GetProjectGroupsFilteredReportWithHistory(ProjectGroupFilteredReportListViewModel request, 
            DateTime startDate, DateTime dueDate, Interval intervalType, IList<ExcelTableCell> tableHeaders)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2,
                    tableHeaders.Where(x => x.IsActive).Select(x => x.Width).ToList());

                var startInterval = (int)Math.Ceiling((12 * startDate.Year + startDate.Month) / ((float)intervalType));
                var endInclusiveInterval = (int)Math.Ceiling((12 * dueDate.Year + dueDate.Month) / ((float)intervalType));

                var tableHelper = new ExcelTableHelper();
                var headers = tableHelper.GetProjectGroupsWithHistoryFilteredReportTableHeaders();
                var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
                var periodName = tableNames.FirstOrDefault(x => x == headers[4].Name);
                int order = 0;
                if (!string.IsNullOrWhiteSpace(periodName))
                {
                    order = tableNames.IndexOf(periodName);
                    var temporaryLeft = tableNames.GetRange(0, order);
                    var temporaryRight = tableNames.GetRange(order + 1, tableNames.Count - order - 1);
                    var temporaryMiddle = new List<string>();
                    for (var intervalOrder = startInterval - 1; intervalOrder++ < endInclusiveInterval;)
                    {
                        temporaryMiddle.Add($"{(intervalOrder - 1) * (int)intervalType / 12} Year, " +
                                            $"{Math.Ceiling(intervalOrder % (12 / (int)intervalType) == 0 ? (12 / (float)intervalType) : intervalOrder % (12 / (int)intervalType) * 1.0f)} {intervalType.ToString()}");
                    }

                    tableNames = temporaryLeft;
                    tableNames.AddRange(temporaryMiddle);
                    tableNames.AddRange(temporaryRight);
                }

                var leftAlignmentColumns = new List<int>();
                var rightAlignmentColumns = new List<int>();
                var centerAlignmentColumns = new List<int>();

                for (var i = 0; i < tableHeaders.Count;)
                {
                    switch (tableHeaders[i++].Alignment)
                    {
                        case ExcelHorizontalAlignment.Left:
                            leftAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Right:
                            rightAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Center:
                            centerAlignmentColumns.Add(i);
                            break;
                    }
                }

                for (var i = 2; i <= tableNames.Count; i++)
                {
                    rightAlignmentColumns.Add(i);
                }

                for (var i = tableHeaders.Count; i < tableNames.Count + 2; i++)
                {
                    worksheet.Column(i).Width = 40;
                }

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, leftAlignmentColumns);
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, centerAlignmentColumns);
                SetXmlColumnRightHorizontalAlignment(ref worksheet, rightAlignmentColumns);

                var startRow = 2;
                var row = 2;

                var startColumn = "B";

                SetXmlTableHeader(ref worksheet, row, startColumn, tableNames);
                SetXmlColumnsBold(ref worksheet, row, startColumn, tableNames.Count);
                SetXmlBottomBorderStyle(ref worksheet, startRow, startColumn, tableNames.Count, ExcelBorderStyle.Thin);

                var projectNameColumn = "A";
                var projectGroupNameColumn = "A";

                foreach (var projectGroup in request.ProjectGroups)
                {
                    var isSettedProjectGroup = false;

                    foreach (var project in projectGroup.ProjectView.Projects)
                    {
                        startRow = row;
                        var isSettedProject = false;

                        foreach (var assignee in project.AssigneeView.Assignees)
                        {
                            ++row;
                            var currentColumn = "B";
                            var periodColumn = 0;

                            foreach (var name in tableNames)
                            {
                                var pattern = $"{currentColumn}{row}";
                                
                                if (name == headers[0].Name)
                                {
                                    worksheet.Cells[pattern].Value = row - startRow;
                                }
                                else if (name == headers[1].Name)
                                {
                                    if (!isSettedProjectGroup)
                                    {
                                        worksheet.Cells[pattern].Value = projectGroup.ProjectGroupName;
                                        projectGroupNameColumn = currentColumn;
                                        isSettedProjectGroup = true;
                                    }
                                }
                                else if (name == headers[2].Name)
                                {
                                    if (!isSettedProject)
                                    {
                                        worksheet.Cells[pattern].Value = project.ProjectName;
                                        projectNameColumn = currentColumn;
                                        isSettedProject = true;
                                    }
                                }
                                else if (name == headers[3].Name)
                                {
                                    worksheet.Cells[pattern].Value = assignee.AssigneeName;
                                }
                                else if (name == headers[5].Name)
                                {
                                    worksheet.Cells[pattern].Value = assignee.LoggedTimeView.TotalLoggedTime;
                                    worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                }
                                else
                                {
                                    var loggedTime = assignee.LoggedTimeView.LoggedTimes?.FirstOrDefault(x => x.Key == startInterval + periodColumn);
                                    periodColumn++;
                                    if (loggedTime.HasValue && Math.Abs(loggedTime.Value.Value) >= 0.01)
                                    {
                                        var keyValuePair = loggedTime.Value;
                                        worksheet.Cells[pattern].Value = keyValuePair.Value;
                                        worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                    }
                                    else worksheet.Cells[pattern].Value = "-";
                                }

                                currentColumn = currentColumn.Next();
                            }
                        }

                        if (projectNameColumn == "A") continue;
                        worksheet.Cells[$"{projectNameColumn}{++row}"].Value =
                            $"Total per project: {project.AssigneeView.TotalLoggedTime:0.00}";
                        worksheet.Cells[$"{projectNameColumn}{row}"].Style.Font.Bold = true;
                    }

                    if (projectGroupNameColumn != "A")
                    {
                        worksheet.Cells[$"{projectGroupNameColumn}{++row}"].Value =
                            $"Total per project group: {projectGroup.ProjectView.TotalLoggedTime:0.00}";
                        worksheet.Cells[$"{projectGroupNameColumn}{row}"].Style.Font.Bold = true;
                    }

                    row += 2;
                }

                return package.GetAsByteArray();
            }
        }

        public byte[] GetTeamsByFiltersReport(TeamsFilteredReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2,
                    tableHeaders.Where(x => x.IsActive).Select(x => x.Width).ToList());

                var leftAlignmentColumns = new List<int>();
                var rightAlignmentColumns = new List<int>();
                var centerAlignmentColumns = new List<int>();

                for (var i = 0; i < tableHeaders.Count;)
                {
                    switch (tableHeaders[i++].Alignment)
                    {
                        case ExcelHorizontalAlignment.Left:
                            leftAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Right:
                            rightAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Center:
                            centerAlignmentColumns.Add(i);
                            break;
                    }
                }

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, leftAlignmentColumns);
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, centerAlignmentColumns);
                SetXmlColumnRightHorizontalAlignment(ref worksheet, rightAlignmentColumns);

                var startRow = 2;
                var row = 2;

                var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
                var tableHelper = new ExcelTableHelper();
                var headers = tableHelper.GetTeamsByFiltersReportTableHeaders();
                var startColumn = 'B';

                SetXmlTableHeader(ref worksheet, row, startColumn, tableNames);
                SetXmlColumnsBold(ref worksheet, row, startColumn, tableNames.Count);
                SetXmlBottomBorderStyle(ref worksheet, startRow, startColumn, tableNames.Count, ExcelBorderStyle.Thin);

                var loggedHoursColumn = 'A';
                var estimatedHoursColumn = 'A';
                foreach (var assignee in request.Assignees)
                {
                    bool isSettedAssignee = false;
                    foreach (var activity in assignee.ActivityView.Activities)
                    {
                        foreach (var loggedTime in activity.LoggedTimeView.LoggedTimes)
                        {
                            ++row;
                            var currentColumn = startColumn;

                            foreach (var name in tableNames)
                            {
                                var pattern = $"{currentColumn++}{row}";

                                if (name == headers[0].Name)
                                {
                                    worksheet.Cells[pattern].Value = row - startRow;
                                }
                                else if (name == headers[1].Name)
                                {
                                    if (isSettedAssignee) continue;
                                    worksheet.Cells[pattern].Value = assignee.AssigneeName;
                                    isSettedAssignee = true;
                                }
                                else if (name == headers[2].Name)
                                {
                                    worksheet.Cells[pattern].Value = loggedTime.ProjectName;
                                }
                                else if (name == headers[3].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.ActivityName;
                                }
                                else if (name == headers[4].Name)
                                {
                                    worksheet.Cells[pattern].Value = loggedTime.ActivityStatus;
                                }
                                else if (name == headers[5].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.LoggedTimeView.TotalEstimatedTime;
                                    worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                    estimatedHoursColumn = currentColumn;
                                }
                                else if (name == headers[6].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.LoggedTimeView.TotalLoggedTime;
                                    worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                    loggedHoursColumn = currentColumn;
                                }
                            }
                        }
                    }

                    var rowForTotal = row + 1;

                    if (estimatedHoursColumn != 'A' && loggedHoursColumn != 'A')
                    {
                        worksheet.Cells[$"A{rowForTotal}"].Value = "Total:";
                        worksheet.Cells[$"A{rowForTotal}"].Style.Font.Bold = true;

                        if (estimatedHoursColumn != 'A')
                        {
                            worksheet.Cells[$"{--estimatedHoursColumn}{rowForTotal}"].Value = assignee.ActivityView.TotalEstimatedTime;
                            worksheet.Cells[$"{estimatedHoursColumn}{rowForTotal}"].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[$"{estimatedHoursColumn}{rowForTotal}"].Style.Font.Bold = true;
                        }

                        if (loggedHoursColumn != 'A')
                        {
                            worksheet.Cells[$"{--loggedHoursColumn}{rowForTotal}"].Value = assignee.ActivityView.TotalLoggedTime;
                            worksheet.Cells[$"{loggedHoursColumn}{rowForTotal}"].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[$"{loggedHoursColumn}{rowForTotal}"].Style.Font.Bold = true;
                        }
                    }

                    startRow = row = rowForTotal + 1;
                }

                var rowForFinal = row + 1;

                if (estimatedHoursColumn == 'A' || loggedHoursColumn == 'A') return package.GetAsByteArray();
                worksheet.Cells[$"A{rowForFinal}"].Value = "Total:";
                worksheet.Cells[$"A{rowForFinal}"].Style.Font.Bold = true;

                if (estimatedHoursColumn != 'A')
                {
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Value = request.TotalEstimatedTime;
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Style.Font.Bold = true;
                }

                if (loggedHoursColumn == 'A') return package.GetAsByteArray();
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Value = request.TotalLoggedTime;
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Style.Numberformat.Format = "0.00";
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Style.Font.Bold = true;

                return package.GetAsByteArray();
            }
        }

        public byte[] GetEmployeeFilteredReport(EmployeeFilteredReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2,
                    tableHeaders.Where(x => x.IsActive).Select(x => x.Width).ToList());

                var leftAlignmentColumns = new List<int>();
                var rightAlignmentColumns = new List<int>();
                var centerAlignmentColumns = new List<int>();

                for (var i = 0; i < tableHeaders.Count;)
                {
                    switch (tableHeaders[i++].Alignment)
                    {
                        case ExcelHorizontalAlignment.Left:
                            leftAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Right:
                            rightAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Center:
                            centerAlignmentColumns.Add(i);
                            break;
                    }
                }

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, leftAlignmentColumns);
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, centerAlignmentColumns);
                SetXmlColumnRightHorizontalAlignment(ref worksheet, rightAlignmentColumns);

                var startRow = 2;
                var row = 2;

                var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
                var tableHelper = new ExcelTableHelper();
                var headers = tableHelper.GetTeamsByFiltersReportTableHeaders();

                var startColumn = 'B';

                SetXmlTableHeader(ref worksheet, row, startColumn, tableNames);
                SetXmlColumnsBold(ref worksheet, row, startColumn, tableNames.Count);
                SetXmlBottomBorderStyle(ref worksheet, startRow, startColumn, tableNames.Count, ExcelBorderStyle.Thin);

                var loggedHoursColumn = 'A';
                var estimatedHoursColumn = 'A';
                foreach (var assignee in request.Assignees)
                {
                    bool isSettedAssignee = false;
                    foreach (var activity in assignee.ActivityView.Activities)
                    {
                        foreach (var loggedTime in activity.LoggedTimeView.LoggedTimes)
                        {
                            ++row;
                            var currentColumn = startColumn;

                            foreach (var name in tableNames)
                            {
                                var pattern = $"{currentColumn++}{row}";

                                if (name == headers[0].Name)
                                {
                                    worksheet.Cells[pattern].Value = row - startRow;
                                }
                                else if (name == headers[1].Name)
                                {
                                    if (isSettedAssignee) continue;
                                    worksheet.Cells[pattern].Value = assignee.AssigneeName;
                                    isSettedAssignee = true;
                                }
                                else if (name == headers[2].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.ProjectName;
                                }
                                else if (name == headers[3].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.ActivityName;
                                }
                                else if (name == headers[4].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.ActivityStatus;
                                }
                                else if (name == headers[5].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.LoggedTimeView.TotalEstimatedTime;
                                    worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                    estimatedHoursColumn = currentColumn;
                                }
                                else if (name == headers[6].Name)
                                {
                                    worksheet.Cells[pattern].Value = activity.LoggedTimeView.TotalLoggedTime;
                                    worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                    loggedHoursColumn = currentColumn;
                                }
                            }
                        }
                    }

                    var rowForTotal = row + 1;

                    if (estimatedHoursColumn != 'A' && loggedHoursColumn != 'A')
                    {
                        worksheet.Cells[$"A{rowForTotal}"].Value = "Total:";
                        worksheet.Cells[$"A{rowForTotal}"].Style.Font.Bold = true;

                        if (estimatedHoursColumn != 'A')
                        {
                            worksheet.Cells[$"{--estimatedHoursColumn}{rowForTotal}"].Value = assignee.ActivityView.TotalEstimatedTime;
                            worksheet.Cells[$"{estimatedHoursColumn}{rowForTotal}"].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[$"{estimatedHoursColumn}{rowForTotal}"].Style.Font.Bold = true;
                        }

                        if (loggedHoursColumn != 'A')
                        {
                            worksheet.Cells[$"{--loggedHoursColumn}{rowForTotal}"].Value = assignee.ActivityView.TotalLoggedTime;
                            worksheet.Cells[$"{loggedHoursColumn}{rowForTotal}"].Style.Numberformat.Format = "0.00";
                            worksheet.Cells[$"{loggedHoursColumn}{rowForTotal}"].Style.Font.Bold = true;
                        }
                    }

                    startRow = row = rowForTotal + 1;
                }

                var rowForFinal = row + 1;

                if (estimatedHoursColumn == 'A' || loggedHoursColumn == 'A') return package.GetAsByteArray();
                worksheet.Cells[$"A{rowForFinal}"].Value = "Total:";
                worksheet.Cells[$"A{rowForFinal}"].Style.Font.Bold = true;

                if (estimatedHoursColumn != 'A')
                {
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Value = request.TotalEstimatedTime;
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Style.Font.Bold = true;
                }

                if (loggedHoursColumn == 'A') return package.GetAsByteArray();
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Value = request.TotalLoggedTime;
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Style.Numberformat.Format = "0.00";
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Style.Font.Bold = true;

                return package.GetAsByteArray();
            }
        }

        public byte[] GetGeneralFilteredReport(GeneralFilteredReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2,
                    tableHeaders.Where(x => x.IsActive).Select(x => x.Width).ToList());

                worksheet.Cells["C2"].Value = "General report";
                worksheet.Cells["D2"].Value = $"{DateTime.Now:dddd, dd MMMM yyyy}";

                worksheet.Cells["C2:C2"].Style.Font.Bold = true;

                var leftAlignmentColumns = new List<int>();
                var rightAlignmentColumns = new List<int>();
                var centerAlignmentColumns = new List<int>();

                for (var i = 0; i < tableHeaders.Count;)
                {
                    switch (tableHeaders[i++].Alignment)
                    {
                        case ExcelHorizontalAlignment.Left:
                            leftAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Right:
                            rightAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Center:
                            centerAlignmentColumns.Add(i);
                            break;
                    }
                }

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, leftAlignmentColumns);
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, centerAlignmentColumns);
                SetXmlColumnRightHorizontalAlignment(ref worksheet, rightAlignmentColumns);

                var startRow = 4;
                var row = 4;

                var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
                var tableHelper = new ExcelTableHelper();
                var headers = tableHelper.GetGeneralFilteredReportTableHeaders();

                var startColumn = 'B';

                SetXmlTableHeader(ref worksheet, row, startColumn, tableNames);
                SetXmlColumnsBold(ref worksheet, row, startColumn, tableNames.Count);
                SetXmlBottomBorderStyle(ref worksheet, startRow, startColumn, tableNames.Count, ExcelBorderStyle.Thin);

                var estimatedHoursColumn = 'A';
                var loggedHoursColumn = 'A';

                var projectGroupColumn = 'A';
                var projectColumn = 'A';
                var sprintColumn = 'A';
                var activityColumn = 'A';

                foreach (var projectGroup in request.ProjectGroups)
                {
                    var isProjectGroupSetted = false;
                    foreach(var project in projectGroup.ProjectView.Projects)
                    {
                        var isProjectSetted = false;
                        foreach (var sprint in project.SprintView.Sprints)
                        {
                            var isSprintSetted = false;
                            foreach (var activity in sprint.ActivityView.Activities)
                            {
                                var isActivitySetted = false;
                                foreach (var assignee in activity.AssigneeView.Assignees)
                                {
                                    ++row;
                                    var currentColumn = startColumn;

                                    foreach (var name in tableNames)
                                    {
                                        var pattern = $"{currentColumn++}{row}";

                                        if (name == headers[0].Name)
                                        {
                                            worksheet.Cells[pattern].Value = row - startRow;
                                        }
                                        else if (name == headers[1].Name)
                                        {
                                            if (isProjectGroupSetted) continue;
                                            worksheet.Cells[pattern].Value = projectGroup.ProjectGroupName;
                                            isProjectGroupSetted = true;
                                            projectGroupColumn = currentColumn;
                                        }
                                        else if (name == headers[2].Name)
                                        {
                                            if (isProjectSetted) continue;
                                            worksheet.Cells[pattern].Value = project.ProjectName;
                                            isProjectSetted = true;
                                            projectColumn = currentColumn;
                                        }
                                        else if (name == headers[3].Name)
                                        {
                                            if (isSprintSetted) continue;
                                            worksheet.Cells[pattern].Value = sprint.SprintName;
                                            isSprintSetted = true;
                                            sprintColumn = currentColumn;
                                        }
                                        else if (name == headers[4].Name)
                                        {
                                            if (isActivitySetted) continue;
                                            worksheet.Cells[pattern].Value = activity.ActivityName;
                                            isActivitySetted = true;
                                            activityColumn = currentColumn;
                                        }
                                        else if (name == headers[5].Name)
                                        {
                                            worksheet.Cells[pattern].Value = assignee.AssigneeName;
                                        }
                                        else if (name == headers[6].Name)
                                        {
                                            worksheet.Cells[pattern].Value = activity.EstimatedTime;
                                            worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                            estimatedHoursColumn = currentColumn;
                                        }
                                        else if (name == headers[7].Name)
                                        {
                                            worksheet.Cells[pattern].Value = activity.AssigneeView.TotalLoggedTime;
                                            worksheet.Cells[pattern].Style.Numberformat.Format = "0.00";
                                            loggedHoursColumn = currentColumn;
                                        }
                                    }
                                }

                                if (activityColumn != 'A' && (estimatedHoursColumn != 'A' || loggedHoursColumn != 'A'))
                                {
                                    ++startRow;
                                    ++row;
                                    worksheet.Cells[$"{--activityColumn}{row}"].Value = "Total:";
                                    worksheet.Cells[$"{activityColumn}{row}"].Style.Font.Bold = true;

                                    if (estimatedHoursColumn != 'A')
                                    {
                                        worksheet.Cells[$"{--estimatedHoursColumn}{row}"].Value = activity.EstimatedTime;
                                        worksheet.Cells[$"{estimatedHoursColumn}{row}"].Style.Font.Bold = true;
                                        worksheet.Cells[$"{estimatedHoursColumn}{row}"].Style.Font.Bold = true;
                                        ++estimatedHoursColumn;
                                    }
                                    if (loggedHoursColumn != 'A')
                                    {
                                        worksheet.Cells[$"{--loggedHoursColumn}{row}"].Value = activity.AssigneeView.TotalLoggedTime;
                                        worksheet.Cells[$"{loggedHoursColumn}{row}"].Style.Font.Bold = true;
                                        worksheet.Cells[$"{loggedHoursColumn}{row}"].Style.Font.Bold = true;
                                        ++loggedHoursColumn;
                                    }
                                }
                            }

                            if (sprintColumn != 'A' && (estimatedHoursColumn != 'A' || loggedHoursColumn != 'A'))
                            {
                                ++startRow;
                                ++row;
                                worksheet.Cells[$"{--sprintColumn}{row}"].Value = "Total:";
                                worksheet.Cells[$"{sprintColumn}{row}"].Style.Font.Bold = true;

                                if (estimatedHoursColumn != 'A')
                                {
                                    worksheet.Cells[$"{--estimatedHoursColumn}{row}"].Value = sprint.ActivityView.TotalEstimatedTime;
                                    worksheet.Cells[$"{estimatedHoursColumn}{row}"].Style.Font.Bold = true;
                                    worksheet.Cells[$"{estimatedHoursColumn}{row}"].Style.Font.Bold = true;
                                    ++estimatedHoursColumn;
                                }
                                if (loggedHoursColumn != 'A')
                                {
                                    worksheet.Cells[$"{--loggedHoursColumn}{row}"].Value = sprint.ActivityView.TotalLoggedTime;
                                    worksheet.Cells[$"{loggedHoursColumn}{row}"].Style.Font.Bold = true;
                                    worksheet.Cells[$"{loggedHoursColumn}{row}"].Style.Font.Bold = true;
                                    ++loggedHoursColumn;
                                }
                            }
                        }

                        if (projectColumn != 'A' && (estimatedHoursColumn != 'A' || loggedHoursColumn != 'A'))
                        {
                            ++startRow;
                            ++row;
                            worksheet.Cells[$"{--projectColumn}{row}"].Value = "Total:";
                            worksheet.Cells[$"{projectColumn}{row}"].Style.Font.Bold = true;

                            if (estimatedHoursColumn != 'A')
                            {
                                worksheet.Cells[$"{--estimatedHoursColumn}{row}"].Value = project.SprintView.TotalEstimatedTime;
                                worksheet.Cells[$"{estimatedHoursColumn}{row}"].Style.Font.Bold = true;
                                worksheet.Cells[$"{estimatedHoursColumn}{row}"].Style.Font.Bold = true;
                                ++estimatedHoursColumn;
                            }
                            if (loggedHoursColumn != 'A')
                            {
                                worksheet.Cells[$"{--loggedHoursColumn}{row}"].Value = project.SprintView.TotalLoggedTime;
                                worksheet.Cells[$"{loggedHoursColumn}{row}"].Style.Font.Bold = true;
                                worksheet.Cells[$"{loggedHoursColumn}{row}"].Style.Font.Bold = true;
                                ++loggedHoursColumn;
                            }
                        }
                    }

                    if (projectGroupColumn != 'A' && (estimatedHoursColumn != 'A' || loggedHoursColumn != 'A'))
                    {
                        ++startRow;
                        ++row;
                        worksheet.Cells[$"{--projectGroupColumn}{row}"].Value = "Total:";
                        worksheet.Cells[$"{projectGroupColumn}{row}"].Style.Font.Bold = true;

                        if (estimatedHoursColumn != 'A')
                        {
                            worksheet.Cells[$"{--estimatedHoursColumn}{row}"].Value = projectGroup.ProjectView.TotalEstimatedTime;
                            worksheet.Cells[$"{estimatedHoursColumn}{row}"].Style.Font.Bold = true;
                            worksheet.Cells[$"{estimatedHoursColumn}{row}"].Style.Font.Bold = true;
                            ++estimatedHoursColumn;
                        }
                        if (loggedHoursColumn != 'A')
                        {
                            worksheet.Cells[$"{--loggedHoursColumn}{row}"].Value = projectGroup.ProjectView.TotalLoggedTime;
                            worksheet.Cells[$"{loggedHoursColumn}{row}"].Style.Font.Bold = true;
                            worksheet.Cells[$"{loggedHoursColumn}{row}"].Style.Font.Bold = true;
                            ++loggedHoursColumn;
                        }
                    }

                    startRow = row;
                }

                var rowForFinal = row + 1;

                if (estimatedHoursColumn == 'A' && loggedHoursColumn == 'A') return package.GetAsByteArray();
                worksheet.Cells[$"A{rowForFinal}"].Value = "Total:";
                worksheet.Cells[$"A{rowForFinal}"].Style.Font.Bold = true;

                if (estimatedHoursColumn != 'A')
                {
                    worksheet.Cells[$"{--estimatedHoursColumn}{rowForFinal}"].Value = request.TotalEstimatedTime;
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[$"{estimatedHoursColumn}{rowForFinal}"].Style.Font.Bold = true;
                }

                if (loggedHoursColumn == 'A') return package.GetAsByteArray();
                worksheet.Cells[$"{--loggedHoursColumn}{rowForFinal}"].Value = request.TotalLoggedTime;
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Style.Numberformat.Format = "0.00";
                worksheet.Cells[$"{loggedHoursColumn}{rowForFinal}"].Style.Font.Bold = true;

                return package.GetAsByteArray();
            }
        }

        public byte[] GetOverdueTasksFilteredReport(OverdueTasksFilteredReportViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            using (var package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");

                SetXmlColumnsWidth(ref worksheet, 2,
                    tableHeaders.Where(x => x.IsActive).Select(x => x.Width).ToList());

                var leftAlignmentColumns = new List<int>();
                var rightAlignmentColumns = new List<int>();
                var centerAlignmentColumns = new List<int>();

                for (var i = 0; i < tableHeaders.Count;)
                {
                    switch (tableHeaders[i++].Alignment)
                    {
                        case ExcelHorizontalAlignment.Left:
                            leftAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Right:
                            rightAlignmentColumns.Add(i);
                            break;
                        case ExcelHorizontalAlignment.Center:
                            centerAlignmentColumns.Add(i);
                            break;
                    }
                }

                SetXmlColumnLeftHorizontalAlignment(ref worksheet, leftAlignmentColumns);
                SetXmlColumnCenterHorizontalAlignment(ref worksheet, centerAlignmentColumns);
                SetXmlColumnRightHorizontalAlignment(ref worksheet, rightAlignmentColumns);

                var startRow = 2;
                var row = 2;

                var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
                var tableHelper = new ExcelTableHelper();

                var headers = tableHelper.GetOverdueTasksFilteredReportTableHeaders();
                var startColumn = 'B';

                SetXmlTableHeader(ref worksheet, row, startColumn, tableNames);
                SetXmlColumnsBold(ref worksheet, row, startColumn, tableNames.Count);
                SetXmlBottomBorderStyle(ref worksheet, startRow, startColumn, tableNames.Count, ExcelBorderStyle.Thin);

                foreach (var projectGroup in request.ProjectGroups)
                {
                    var isProjectGroupSetted = false;
                    foreach (var project in projectGroup.Projects)
                    {
                        var isProjectSetted = false;
                        foreach (var activity in project.Activities)
                        {
                            var isActivitySetted = false;
                            foreach (var assignee in activity.Assignees)
                            {
                                ++row;
                                var currentColumn = startColumn;

                                foreach (var name in tableNames)
                                {
                                    var pattern = $"{currentColumn++}{row}";

                                    if (name == headers[0].Name)
                                    {
                                        worksheet.Cells[pattern].Value = row - startRow;
                                    }
                                    else if (name == headers[1].Name)
                                    {
                                        if (isProjectGroupSetted) continue;
                                        worksheet.Cells[pattern].Value = projectGroup.ProjectGroupName;
                                        isProjectGroupSetted = true;
                                    }
                                    else if (name == headers[2].Name)
                                    {
                                        if (isProjectSetted) continue;
                                        worksheet.Cells[pattern].Value = project.ProjectName;
                                        isProjectSetted = true;
                                    }
                                    else if (name == headers[3].Name)
                                    {
                                        if (isActivitySetted) continue;
                                        worksheet.Cells[pattern].Value = activity.ActivityName;
                                        isActivitySetted = true;
                                    }
                                    else if (name == headers[4].Name)
                                    {
                                        worksheet.Cells[pattern].Value = assignee.AssigneeName;
                                    }
                                    else if (name == headers[5].Name)
                                    {
                                        if (assignee.Deadline.HasValue)
                                        {
                                            worksheet.Cells[pattern].Value = assignee.Deadline;
                                            worksheet.Cells[pattern].Value = $"{assignee.Deadline:dddd, dd MMMM yyyy}";
                                        }
                                        else
                                        {
                                            worksheet.Cells[pattern].Value = "Deadline not setted";
                                        }
                                    }
                                    else if (name == headers[6].Name)
                                    {
                                        worksheet.Cells[pattern].Value = assignee.Overdue;
                                    }
                                }
                            }
                        }
                    }
                }

                return package.GetAsByteArray();
            }
        }
    }
}
