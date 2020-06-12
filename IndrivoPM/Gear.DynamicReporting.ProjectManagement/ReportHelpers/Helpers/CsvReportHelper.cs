using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gear.DynamicReporting.Abstractions.Extensions;
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

namespace Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers
{
    /// <summary>
    /// Generates dynamic .csv reports. By default it is opened in excel via comma-separated values.
    /// Each method returns array of bytes (byte[]). The content is created with help of StringBuilder 
    /// and after that is encoded with help of Encoding class (using Unicode). The default value for 
    /// Maximal Capacity of the StringBuilder is Int32.MaxValue (as a mention).
    /// </summary>
    public partial class CsvReportHelper : IReportHelper<ExcelTableCell>
    {
        #region Csv Private Methods
        /// <summary>
        /// Creates the common part for general report.
        /// </summary>
        /// <param name="request">The class which contains necessary data.</param>
        /// <param name="startDate">Represents start date.</param>
        /// <param name="dueDate">Represents due date</param>
        /// <returns></returns>
        private byte[] CsvForGeneralReport(LoggedTimeByPeriodListViewModel request, DateTime startDate, DateTime dueDate)
        {
            var result = new StringBuilder();

            result.AppendFormat("{0}, {1}{2}", "Start date", $"{startDate:dddd, dd MMMM yyyy}", '\n');
            result.AppendFormat("{0}, {1}{2}", "Due date", $"{dueDate:dddd, dd MMMM yyyy}", '\n');

            result.Append('\n');

            result.AppendFormat("{0}, {1}, {2}, {3}{4}",
                "#", "Employee", "Est.", "Log.", '\n');

            int iteration = 0;
            foreach (var loggedTime in request.UsersLoggedEstimatedTime)
            {
                result.AppendFormat("{0}, {1}, {2}, {3}{4}", ++iteration, loggedTime.Name,
                    loggedTime.UserEstimatedTimeByPeriod, loggedTime.UserLoggedTimeByPeriod, '\n');
            }

            result.AppendFormat("{0}, {1}, {2}{3}", "Total time", request.TotalEstimatedTime, request.TotalLoggedTime, '\n');

            return Encoding.Unicode.GetBytes(result.ToString());
        }
        #endregion

        public byte[] GetEmployeeLoggedTime(UserLoggedTimeListViewModel request, DateTime startDate, DateTime dueDate)
        {
            var result = new StringBuilder();

            result.AppendFormat("{0}, {1}{2}", "Employee", request.UserName, '\n');
            result.AppendFormat("{0}, {1}{2}", "Start date", $"{startDate:dddd, dd MMMM yyyy}", '\n');
            result.AppendFormat("{0} {1}{2}", "Due date", $"{dueDate:dddd, dd MMMM yyyy}", '\n');

            result.Append('\n');

            result.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}{6}",
                "#", "Activity", "Project", "Subtype", "Log.", "Date", '\n');

            int iteration = 0;
            foreach (var loggedTime in request.LoggedTimes)
            {
                result.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}{6}", ++iteration, loggedTime.ActivityName,
                    loggedTime.ProjectName, loggedTime.TrackerName, loggedTime.Time,
                    $"{loggedTime.DateOfWork:dddd, dd MMMM yyyy}", '\n');
            }

            result.AppendFormat("{0}, {1}{2}", "Total logged time", request.TotalLoggedTime, '\n');

            result.Append('\n');

            result.AppendFormat("{0}, {1}{2}", "Estimated time (total)", request.TotalEstimatedTime, '\n');
            result.AppendFormat("{0}, {1}{2}", "Logged time (total)", request.TotalLoggedTime, '\n');

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetEmployeesLoggedTimeByPeriod(LoggedTimeByPeriodListViewModel request, DateTime startDate, DateTime dueDate)
        {
            return CsvForGeneralReport(request, startDate, dueDate);
        }

        public byte[] GetProjectGeneralReport(ProjectGeneralReportListViewModel request, DateTime startDate, DateTime dueDate)
        {
            var result = new StringBuilder();

            result.AppendFormat("{0}, {1}{2}", "Project", request.ProjectName, '\n');
            result.AppendFormat("{0}, {1}{2}", "Start date", $"{startDate:dddd, dd MMMM yyyy}", '\n');
            result.AppendFormat("{0}, {1}{2}", "Due date", $"{dueDate:dddd, dd MMMM yyyy}", '\n');

            result.Append('\n');

            result.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}{11}",
                "#", "Activity List", "Sprint", "Employee", "Activity", "Priority", "Est.", "Log.", "Progress", "Status", "Modified", '\n');

            int iteration = 0;
            foreach (var activity in request.Activities)
            {
                result.AppendFormat("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}{11}", ++iteration, activity.ActivityListName,
                    activity.SprintName, activity.EmployeeName, activity.ActivityName, activity.ActivityPriority, activity.Estimated,
                    activity.Logged, activity.Progress, activity.ActivityStatus, string.Format("{0:dddd, dd MMMM yyyy}", activity.LastModified), '\n');
            }

            result.AppendFormat("{0}, {1}, {2}{3}", "Total time", request.TotalEstimatedTime, request.TotalLoggedTime, '\n');

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetProjectGroupsListReport(ProjectGroupsGeneralReportListViewModel request, DateTime startDate, DateTime dueDate)
        {
            var result = new StringBuilder();

            result.AppendFormat("{0}, {1}{2}", "Start date", $"{startDate:dddd, dd MMMM yyyy}", '\n');
            result.AppendFormat("{0}, {1}{2}", "Due date", $"{dueDate:dddd, dd MMMM yyyy}", '\n');

            result.Append('\n');

            foreach (var projectGroup in request.ProjectGroups)
            {
                result.AppendFormat("{0}, {1}{2}", "Project Group: ", projectGroup.ProjectGroupName, '\n');

                foreach (var project in projectGroup.Projects)
                {
                    result.AppendFormat("{0}, {1}, {2}, {3}{4}", "#", "Project", "Est.", "Log.", '\n');
                }
                result.AppendFormat("{0}, {1}, {2}{3}", "Total time", projectGroup.TotalEstimatedTime, projectGroup.TotalLoggedTime, '\n');

                result.Append("\n\n\n");
            }

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetProjectListReport(ProjectLoggedTimeReportListViewModel request, DateTime startDate, DateTime dueDate)
        {
            var result = new StringBuilder();

            // Write start and end date.
            result.AppendFormat("{0}, {1}{2}", "Start date", $"{startDate:dddd, dd MMMM yyyy}", '\n');
            result.AppendFormat("{0}, {1}{2}", "Due date", $"{dueDate:dddd, dd MMMM yyyy}", '\n');

            result.Append('\n');

            // Write header.
            result.AppendFormat("{0}, {1}, {2}, {3}{4}", "#", "Project", "Est.", "Log.", '\n');

            int row = 1;

            foreach (var project in request.ProjectReports)
            {
                result.AppendFormat($"{row}, {project.ProjectName}, {project.EstimatedTime}, {project.LoggedTime} \n");
                row++;
            }

            result.AppendFormat("{0}, {1}, {2}{3}", "Total time", request.TotalEstimatedTime, request.TotalLoggedTime, '\n');

            result.Append("\n\n\n");

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetActivityListsByProjectReport(ActivityListByProjectReportListViewModel request)
        {
            var result = new StringBuilder();

            result.AppendFormat("Project:, {0}{1}", request.ProjectName, '\n');
            result.Append('\n');

            result.AppendFormat("{0}, {1}", '#', "Activity list name");
            foreach (var activityTypes in request.ActivityTypes)
            {
                result.AppendFormat(", {0}%", activityTypes);
            }
            result.AppendFormat(", {0}, {1}, {2}, {3}{4}", "Avg.%", "Status", "Planned date", "Actual date", '\n');

            var iteration = 0;
            foreach (var activityList in request.ActivityList)
            {
                result.AppendFormat("{0}, {1}", ++iteration, activityList.ActivityListName);
                foreach (var activityType in activityList.ActivityTypes)
                {
                    if (activityType.Progress.HasValue)
                    {
                        result.AppendFormat(", {0}%", activityType.Progress);
                    }
                    else
                    {
                        result.Append(", -");
                    }
                }

                if (activityList.Average.HasValue)
                {
                    result.AppendFormat(", {0}%", activityList?.Average.Value);
                }
                else
                {
                    result.Append(", -");
                }

                var status = "Completed";
                if (!activityList.Average.HasValue || activityList.Average.Value == 0)
                {
                    status = "New";
                }
                else if (activityList.Average.Value < 100)
                {
                    status = "Ongoing";
                }
                result.AppendFormat(", {0}", status);

                if (activityList.PlannedDate.HasValue)
                {
                    result.AppendFormat(", {0}", $"{activityList.PlannedDate.Value:dddd, dd MMMM yyyy}");
                }
                else
                {
                    result.Append(", -");
                }

                if (activityList.ActualDate.HasValue)
                {
                    result.AppendFormat(", {0}", $"{activityList.ActualDate.Value:dddd, dd MMMM yyyy}");
                }
                else
                {
                    result.Append(", -");
                }

                result.Append('\n');
            }

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetSprintListGeneralReport(SprintListGeneralReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            var result = new StringBuilder();

            var tableNames = new List<string>
            {
                "#", "ID", "Activity Name", "Assignees", "Status", "Est. time (hours)", "Log. time (hours)", "Comment"
            };

            result.Append($"Project:, {request.ProjectName}\n\n");
            foreach (var sprint in request.Sprints)
            {
                result.Append($"Sprint:, {sprint.SprintName}\n");
                result.Append($"Start Date:, {sprint.StartDate:dddd, dd MMMM yyyy}\n");
                result.Append($"Due Date:, {sprint.DueDate:dddd, dd MMMM yyyy}\n\n");

                result.Append($"{string.Join(", ", tableNames)}\n");
                var column = 0;
                foreach(var activity in sprint.Activities)
                {
                    result.Append(
                        $"{++column}, {string.Format($"A{activity.ActivityNumber:00000}")}, {activity.ActivityName}, {activity.Assignees}" +
                        $", {activity.ActivityStatus}, {activity.EstimatedTime}, {activity.LoggedTime}\n");
                }

                result.Append($"Total:, {sprint.TotalEstimatedTime}, {sprint.TotalLoggedTime}\n\n\n");
            }

            result.Append($"All sprints total: {request.TotalEstimatedTime}, {request.TotalLoggedTime}\n");

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetProjectsByFiltersReport(ProjectFilteredReportListViewModel request,
            IList<ExcelTableCell> tableHeaders)
        {
            var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetProjectsByFiltersReportTableHeaders();

            var result = new StringBuilder();
            result.Append(string.Join(',', tableNames));
            result.AppendLine();

            var row = 0;
            
            foreach (var project in request.Projects)
            {
                foreach (var activity in project.ActivityView.Activities)
                {
                    foreach (var assignee in activity.AssigneeView.Assignees)
                    {
                        var activityStatus = assignee.LoggedTimeView.LoggedTimes[0].ActivityStatus;
                        var activityType = assignee.LoggedTimeView.LoggedTimes[0].ActivityType;

                        foreach (var name in tableNames)
                        {
                            if (name == headers[0].Name)
                            {
                                result.Append($"{++row}, ");
                            }
                            else if (name == headers[1].Name)
                            {
                                result.Append($"{project.ProjectName}, ");
                            }
                            else if (name == headers[2].Name)
                            {
                                result.Append($"{assignee.AssigneeName}, ");
                            }
                            else if (name == headers[3].Name)
                            {
                                result.Append($"{activity.ActivityName}, ");
                            }
                            else if (name == headers[4].Name)
                            {
                                result.Append($"{activityStatus}, ");
                            }
                            else if (name == headers[5].Name)
                            {
                                result.Append($"{activityType}, ");
                            }
                            else if (name == headers[6].Name)
                            {
                                result.Append($"{assignee.LoggedTimeView.TotalEstimatedTime}, ");
                            }
                            else if (name == headers[7].Name)
                            {
                                result.Append($"{assignee.LoggedTimeView.TotalLoggedTime}, ");
                            }
                        }

                        result.AppendLine();
                    }
                }
            }

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetProjectGroupsFilteredReportWithHistory(ProjectGroupFilteredReportListViewModel request, DateTime startDate,
            DateTime dueDate, Interval intervalType, IList<ExcelTableCell> tableHeaders)
        {
            var result = new StringBuilder();

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

            result.AppendLine(string.Join(',', tableNames));
            
            var startRow = 2;
            var row = 2;

            var startColumn = "B";

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
                                result.Append($"{row - startRow}, ");
                            }
                            else if (name == headers[1].Name)
                            {
                                if (!isSettedProjectGroup)
                                {
                                    result.Append($"{projectGroup.ProjectGroupName}, ");
                                    projectGroupNameColumn = currentColumn;
                                    isSettedProjectGroup = true;
                                }
                            }
                            else if (name == headers[2].Name)
                            {
                                if (!isSettedProject)
                                {
                                    result.Append($"{project.ProjectName}, ");
                                    projectNameColumn = currentColumn;
                                    isSettedProject = true;
                                }
                            }
                            else if (name == headers[3].Name)
                            {
                                result.Append($"{assignee.AssigneeName}, ");
                            }
                            else if (name == headers[5].Name)
                            {
                                result.Append($"{assignee.LoggedTimeView.TotalLoggedTime:0.00}, ");
                            }
                            else
                            {
                                var loggedTime = assignee.LoggedTimeView.LoggedTimes?.FirstOrDefault(x => x.Key == startInterval + periodColumn);
                                periodColumn++;
                                if (loggedTime.HasValue && Math.Abs(loggedTime.Value.Value) >= 0.01)
                                {
                                    var keyValuePair = loggedTime.Value;
                                    result.Append($"{keyValuePair.Value:0.00}, ");
                                }
                                else result.Append("-, ");
                            }

                            currentColumn = currentColumn.Next();
                        }

                        result.AppendLine();
                    }

                    result.AppendLine();
                    if (projectNameColumn == "A") continue;
                    result.AppendLine($"Total per project: {project.AssigneeView.TotalLoggedTime:0.00}");
                }

                if (projectGroupNameColumn != "A")
                {
                    result.AppendLine($"Total per project group: {projectGroup.ProjectView.TotalLoggedTime:0.00}");
                }

                row += 2;
                result.AppendLine("\n");
            }

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetTeamsByFiltersReport(TeamsFilteredReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetTeamsByFiltersReportTableHeaders();

            var result = new StringBuilder();
            result.Append(string.Join(',', tableNames));
            result.AppendLine();

            var row = 0;

            foreach (var assignee in request.Assignees)
            {
                bool isSettedAssignee = false;
                foreach (var activity in assignee.ActivityView.Activities)
                {
                    foreach (var loggedTime in activity.LoggedTimeView.LoggedTimes)
                    {
                        ++row;

                        foreach (var name in tableNames)
                        {
                            if (name == headers[0].Name)
                            {
                                result.Append($"{row}, ");
                            }
                            else if (name == headers[1].Name)
                            {
                                if (isSettedAssignee) continue;
                                result.Append($"{assignee.AssigneeName}, ");
                                isSettedAssignee = true;
                            }
                            else if (name == headers[2].Name)
                            {
                                result.Append($"{loggedTime.ProjectName}, ");
                            }
                            else if (name == headers[3].Name)
                            {
                                result.Append($"{activity.ActivityName}, ");
                            }
                            else if (name == headers[4].Name)
                            {
                                result.Append($"{loggedTime.ActivityStatus}, ");
                            }
                            else if (name == headers[5].Name)
                            {
                                result.Append($"{activity.LoggedTimeView.TotalEstimatedTime}, ");
                            }
                            else if (name == headers[6].Name)
                            {
                                result.Append($"{activity.LoggedTimeView.TotalLoggedTime}, ");
                            }
                        }

                        result.AppendLine();
                    }
                }
            }

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetOverdueTasksFilteredReport(OverdueTasksFilteredReportViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            var result = new StringBuilder();

            var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetOverdueTasksFilteredReportTableHeaders();

            var row = 0;

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
                            foreach (var name in tableNames)
                            {
                                if (name == headers[0].Name)
                                {
                                    result.Append(row);
                                }
                                else if (name == headers[1].Name)
                                {
                                    if (isProjectGroupSetted) continue;
                                    result.Append($"{projectGroup.ProjectGroupName}, ");
                                    isProjectGroupSetted = true;
                                }
                                else if (name == headers[2].Name)
                                {
                                    if (isProjectSetted) continue;
                                    result.Append($"{project.ProjectName}, ");
                                    isProjectSetted = true;
                                }
                                else if (name == headers[3].Name)
                                {
                                    if (isActivitySetted) continue;
                                    result.Append($"{activity.ActivityName}, ");
                                    isActivitySetted = true;
                                }
                                else if (name == headers[4].Name)
                                {
                                    result.Append($"{assignee.AssigneeName}, ");
                                }
                                else if (name == headers[5].Name)
                                {
                                    if (assignee.Deadline.HasValue)
                                    {
                                        result.Append($"{assignee.Deadline:dddd, dd MMMM yyyy}, ");
                                    }
                                    else
                                    {
                                        result.Append("\"Deadline not setted\", ");
                                    }
                                }
                                else if (name == headers[6].Name)
                                {
                                    result.Append($"{assignee.Overdue}, ");
                                }
                            }

                            result.AppendLine();
                        }
                    }
                }
            }

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetGeneralFilteredReport(GeneralFilteredReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetGeneralFilteredReportTableHeaders();

            var result = new StringBuilder();
            result.Append(string.Join(',', tableNames));
            result.AppendLine();

            var estimatedHoursColumn = 'A';
            var loggedHoursColumn = 'A';

            var startColumn = 'B';

            var startRow = 0;
            var row = 0;

            foreach (var projectGroup in request.ProjectGroups)
            {
                var isProjectGroupSetted = false;

                foreach (var project in projectGroup.ProjectView.Projects)
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
                                result.AppendLine();
                                var currentColumn = startColumn;

                                foreach (var name in tableNames)
                                {
                                    var pattern = $"{currentColumn++}{row}";

                                    if (name == headers[0].Name)
                                    {
                                        result.Append($"{row - startRow}, ");
                                    }
                                    else if (name == headers[1].Name)
                                    {
                                        if (isProjectGroupSetted) continue;
                                        result.Append($"{projectGroup.ProjectGroupName}, ");
                                        isProjectGroupSetted = true;
                                    }
                                    else if (name == headers[2].Name)
                                    {
                                        if (isProjectSetted) continue;
                                        result.Append($"{project.ProjectName}, ");
                                        isProjectSetted = true;
                                    }
                                    else if (name == headers[3].Name)
                                    {
                                        if (isSprintSetted) continue;
                                        result.Append($"{sprint.SprintName}, ");
                                        isSprintSetted = true;
                                    }
                                    else if (name == headers[4].Name)
                                    {
                                        if (isActivitySetted) continue;
                                        result.Append($"{activity.ActivityName}, ");
                                        isActivitySetted = true;
                                    }
                                    else if (name == headers[5].Name)
                                    {
                                        result.Append($"{assignee.AssigneeName}, ");
                                    }
                                    else if (name == headers[6].Name)
                                    {
                                        result.Append($"{activity.EstimatedTime:0.00}, ");
                                        estimatedHoursColumn = currentColumn;
                                    }
                                    else if (name == headers[7].Name)
                                    {
                                        result.Append($"{activity.AssigneeView.TotalLoggedTime:0.00}, ");
                                        loggedHoursColumn = currentColumn;
                                    }
                                }
                            }

                            if (isActivitySetted && (estimatedHoursColumn != 'A' || loggedHoursColumn != 'A'))
                            {
                                ++startRow;
                                ++row;
                                result.AppendLine();
                                result.Append("Total:, ");

                                if (estimatedHoursColumn != 'A')
                                {
                                    result.Append($"\"Activity estimated: {activity.EstimatedTime:0.00}\", ");
                                    ++estimatedHoursColumn;
                                }
                                if (loggedHoursColumn != 'A')
                                {
                                    result.Append($"\"Activity logged: {activity.AssigneeView.TotalLoggedTime:0.00}\", ");
                                    ++loggedHoursColumn;
                                }
                            }
                        }

                        if (isSprintSetted && (estimatedHoursColumn != 'A' || loggedHoursColumn != 'A'))
                        {
                            ++startRow;
                            ++row;
                            result.AppendLine();
                            result.Append("Total:, ");

                            if (estimatedHoursColumn != 'A')
                            {
                                result.Append($"\"Sprint estimated: {sprint.ActivityView.TotalEstimatedTime:0.00}\", ");
                                ++estimatedHoursColumn;
                            }
                            if (loggedHoursColumn != 'A')
                            {
                                result.Append($"\"Sprint logged: {sprint.ActivityView.TotalLoggedTime:0.00}\", ");
                                ++loggedHoursColumn;
                            }
                        }
                    }

                    if (isProjectSetted && (estimatedHoursColumn != 'A' || loggedHoursColumn != 'A'))
                    {
                        ++startRow;
                        ++row;
                        result.AppendLine();
                        result.Append("Total:, ");

                        if (estimatedHoursColumn != 'A')
                        {
                            result.Append($"\"Project estimated: {project.SprintView.TotalEstimatedTime:0.00}\", ");
                            ++estimatedHoursColumn;
                        }
                        if (loggedHoursColumn != 'A')
                        {
                            result.Append($"\"Project logged: {project.SprintView.TotalLoggedTime:0.00}\", ");
                            ++loggedHoursColumn;
                        }
                    }
                }

                if (isProjectGroupSetted && (estimatedHoursColumn != 'A' || loggedHoursColumn != 'A'))
                {
                    ++startRow;
                    ++row;
                    result.AppendLine();
                    result.Append("Total:, ");

                    if (estimatedHoursColumn != 'A')
                    {
                        result.Append($"\"Project group estimated: {projectGroup.ProjectView.TotalEstimatedTime:0.00}\", ");
                        ++estimatedHoursColumn;
                    }
                    if (loggedHoursColumn != 'A')
                    {
                        result.Append($"\"Project group logged: {projectGroup.ProjectView.TotalLoggedTime:0.00}\", ");
                        ++loggedHoursColumn;
                    }
                }

            }
            startRow = row;
        
            var rowForFinal = row + 1;

            if (estimatedHoursColumn == 'A' && loggedHoursColumn == 'A') return Encoding.Unicode.GetBytes(result.ToString());
            result.AppendLine();
            result.Append("Total:, ");

            if (estimatedHoursColumn != 'A')
            {
                result.Append($"\"All estimated: {request.TotalEstimatedTime:0.00}\", ");
            }

            if (loggedHoursColumn == 'A') return Encoding.Unicode.GetBytes(result.ToString());
            result.Append($"\"All logged: {request.TotalLoggedTime:0.00}\", ");

            return Encoding.Unicode.GetBytes(result.ToString());
        }

        public byte[] GetEmployeeFilteredReport(EmployeeFilteredReportListViewModel request, IList<ExcelTableCell> tableHeaders)
        {
            var tableNames = tableHeaders.Where(x => x.IsActive).Select(x => x.Name).ToList();
            var tableHelper = new ExcelTableHelper();
            var headers = tableHelper.GetTeamsByFiltersReportTableHeaders();

            var result = new StringBuilder();
            result.Append(string.Join(',', tableNames));
            result.AppendLine();

            var row = 0;

            foreach (var assignee in request.Assignees)
            {
                bool isSettedAssignee = false;
                foreach (var activity in assignee.ActivityView.Activities)
                {
                    foreach (var loggedTime in activity.LoggedTimeView.LoggedTimes)
                    {
                        ++row;

                        foreach (var name in tableNames)
                        {
                            if (name == headers[0].Name)
                            {
                                result.Append($"{row}, ");
                            }
                            else if (name == headers[1].Name)
                            {
                                if (isSettedAssignee) continue;
                                result.Append($"{assignee.AssigneeName}, ");
                                isSettedAssignee = true;
                            }
                            else if (name == headers[2].Name)
                            {
                                result.Append($"{activity.ProjectName}, ");
                            }
                            else if (name == headers[3].Name)
                            {
                                result.Append($"{activity.ActivityName}, ");
                            }
                            else if (name == headers[4].Name)
                            {
                                result.Append($"{activity.ActivityStatus}, ");
                            }
                            else if (name == headers[5].Name)
                            {
                                result.Append($"{activity.LoggedTimeView.TotalEstimatedTime}, ");
                            }
                            else if (name == headers[6].Name)
                            {
                                result.Append($"{activity.LoggedTimeView.TotalLoggedTime}, ");
                            }
                        }

                        result.AppendLine();
                    }
                }
            }

            return Encoding.Unicode.GetBytes(result.ToString());
        }
    }
}
