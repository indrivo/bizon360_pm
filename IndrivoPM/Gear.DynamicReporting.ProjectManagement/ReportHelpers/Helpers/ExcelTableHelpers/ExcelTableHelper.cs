using System.Collections.Generic;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Interfaces;
using OfficeOpenXml.Style;

namespace Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.ExcelTableHelpers
{
    public class ExcelTableHelper : IExcelTableHelper
    {
        public List<ExcelTableCell> GetEmployeeLoggedTimeTableHeaders()
        {
            throw new System.NotImplementedException();
        }

        public List<ExcelTableCell> GetEmployeesLoggedTimeByPeriodTableHeaders()
        {
            throw new System.NotImplementedException();
        }

        public List<ExcelTableCell> GetProjectGeneralReportTableHeaders()
        {
            throw new System.NotImplementedException();
        }

        public List<ExcelTableCell> GetProjectGroupsListReportTableHeaders()
        {
            throw new System.NotImplementedException();
        }

        public List<ExcelTableCell> GetActivityListsByProjectReportTableHeaders(List<string> activityTypes)
        {
            throw new System.NotImplementedException();
        }
        
        public List<ExcelTableCell> GetSprintListGeneralReportTableHeaders()
        {
            return new List<ExcelTableCell>
            {
                new ExcelTableCell("#", 1) {Alignment = ExcelHorizontalAlignment.Center, Width = 10},
                new ExcelTableCell("ID", 2) {Alignment = ExcelHorizontalAlignment.Center, Width = 15},
                new ExcelTableCell("Activity Name", 3) {Width = 60},
                new ExcelTableCell("Assignees", 4) {Width = 40},
                new ExcelTableCell("Status", 5) {Width = 20},
                new ExcelTableCell("Est. time (hours)", 6) {Alignment = ExcelHorizontalAlignment.Right, Width = 20},
                new ExcelTableCell("Log. time (hours)", 7) {Alignment = ExcelHorizontalAlignment.Right, Width = 20},
                new ExcelTableCell("Comment", 8) {Alignment = ExcelHorizontalAlignment.Right, Width = 60}
            };
        }

        public List<ExcelTableCell> GetProjectsByFiltersReportTableHeaders()
        {
            return new List<ExcelTableCell>
            {
                new ExcelTableCell("#", 1) {Alignment = ExcelHorizontalAlignment.Center, Width = 10},
                new ExcelTableCell("Project", 2) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Activity name", 3) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Assignee", 4) {Width = 60},
                new ExcelTableCell("Activity status", 5) {Width = 20},
                new ExcelTableCell("Activity type", 6) {Width = 20},
                new ExcelTableCell("Est. time (hours)", 7) {Alignment = ExcelHorizontalAlignment.Right, Width = 20},
                new ExcelTableCell("Log. time (hours)", 8) {Alignment = ExcelHorizontalAlignment.Right, Width = 20}
            };
        }

        public List<ExcelTableCell> GetTeamsByFiltersReportTableHeaders()
        {
            return new List<ExcelTableCell>
            {
                new ExcelTableCell("#", 1) {Alignment = ExcelHorizontalAlignment.Center, Width = 10},
                new ExcelTableCell("Assignee", 2) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Project", 3) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Activity", 4) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Activity status", 5) {Width = 20},
                new ExcelTableCell("Est. time (hours)", 7) {Alignment = ExcelHorizontalAlignment.Right, Width = 20},
                new ExcelTableCell("Log. time (hours)", 8) {Alignment = ExcelHorizontalAlignment.Right, Width = 20}
            };
        }

        public List<ExcelTableCell> GetProjectGroupsWithHistoryFilteredReportTableHeaders()
        {
            return new List<ExcelTableCell>
            {
                new ExcelTableCell("#", 1) {Alignment = ExcelHorizontalAlignment.Center, Width = 10},
                new ExcelTableCell("Project Group", 2) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Project", 3) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Assignee", 4) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Period", 5) {Alignment = ExcelHorizontalAlignment.Right, Width = 40},
                new ExcelTableCell("Total Logged (hours)", 6) {Alignment = ExcelHorizontalAlignment.Right, Width = 40},
            };
        }

        
        public List<ExcelTableCell> GetGeneralFilteredReportTableHeaders()
        {
            return new List<ExcelTableCell>
            {
                new ExcelTableCell("#", 1) {Alignment = ExcelHorizontalAlignment.Center, Width = 10},
                new ExcelTableCell("Project Group", 2) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Project", 3) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Assignee", 4) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Activity", 5) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Sprint", 6) {Alignment = ExcelHorizontalAlignment.Center, Width = 60},
                new ExcelTableCell("Est. time (hours)", 7) {Alignment = ExcelHorizontalAlignment.Right, Width = 20},
                new ExcelTableCell("Log. time (hours)", 7) {Alignment = ExcelHorizontalAlignment.Right, Width = 20},
            };
        }

        public List<ExcelTableCell> GetOverdueTasksFilteredReportTableHeaders()
        {
            return new List<ExcelTableCell>
            {
                new ExcelTableCell("#", 1) { Alignment = ExcelHorizontalAlignment.Center, Width = 10 },
                new ExcelTableCell("Project Group", 2) { Alignment = ExcelHorizontalAlignment.Center, Width = 60 },
                new ExcelTableCell("Project", 3) { Alignment = ExcelHorizontalAlignment.Center, Width = 60 },
                new ExcelTableCell("Activity", 4) { Alignment = ExcelHorizontalAlignment.Center, Width = 60 },
                new ExcelTableCell("Assignee", 5) { Alignment = ExcelHorizontalAlignment.Center, Width = 60 },
                new ExcelTableCell("Deadline", 6) { Alignment = ExcelHorizontalAlignment.Center, Width = 40 },
                new ExcelTableCell("Overdue (days)", 7) { Alignment = ExcelHorizontalAlignment.Center, Width = 30 }
            };
        }
    }
}
