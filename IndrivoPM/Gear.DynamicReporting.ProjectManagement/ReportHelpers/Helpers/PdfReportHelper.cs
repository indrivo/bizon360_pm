using System;
using System.Collections.Generic;
using System.IO;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.Base;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.PdfTableHelpers;
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
using Gear.Manager.Core.EntityServices.Reports.Queries.GetUserLoggedTime;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers
{
    /// <summary>
    /// Used for generating dynamic reports. Each report may contain different format (for example A3, A4 etc.).
    /// Each pdf page should be drawn apart (thus the number of elements per page for each format and their allocation
    /// should be predefined for every report - see the PdfTableHelper class implementation as the example of allocation
    /// of tables per each method).
    /// More information about MigraDoc's PDFsharp see here: http://www.pdfsharp.net/MainPage.ashx
    /// </summary>
    public class PdfReportHelper : BasePdfReportHelper, IReportHelper<PdfTableCell>
    {
        #region Private Methods
        
        private void PopulateForGeneralProjectReport(CommonPdfParameter common, ProjectGeneralReportListViewModel request)
        {
            #region Header
            var section = common.Document.AddSection();
            var header = section.Headers.Primary.AddParagraph();
            header.AddText($"Project: {request.ProjectName}");
            header.Format.Font.Size = common.PdfTableHelper.HeaderSize;
            header.Format.Font.Color = Colors.DarkBlue;
            header.Format.Font.Bold = true;
            header.Format.Font.Italic = true;
            header.Format.Alignment = ParagraphAlignment.Center;
            #endregion

            #region Table

            var tableCells = common.HeaderNames;
            var table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            var iteration = 0;

            // Not all pdf formats may include the same number of table headers
            var addColumn = common.PdfTableHelper.GetType() != typeof(PdfTableHelperA4) ? 1 : 0;
            foreach (var activity in request.Activities)
            {
                var row = table.AddRow();
                row.Cells[0 + addColumn].AddParagraph(activity.ActivityListName);   // Keep the sequence
                row.Cells[0].AddParagraph((++iteration).ToString());                // As it is here
                row.Cells[1 + addColumn].AddParagraph(activity.SprintName);
                row.Cells[2 + addColumn].AddParagraph(activity.EmployeeName);
                row.Cells[3 + addColumn].AddParagraph(activity.ActivityName);
                row.Cells[4 + addColumn].AddParagraph(activity.ActivityPriority.ToString());
                row.Cells[5 + addColumn].AddParagraph(activity.Estimated.ToString("0.00"));
                row.Cells[6 + addColumn].AddParagraph(activity.Logged.ToString("0.00"));
                row.Cells[7 + addColumn].AddParagraph("" + activity.Progress);
                row.Cells[8 + addColumn].AddParagraph(activity.ActivityStatus.ToString());
                row.Cells[9 + addColumn].AddParagraph(string.Format("{0:yyyy-M-d}", activity.LastModified));

                if (iteration % common.PdfTableHelper.ItemsPerPage != 0) continue;

                section = common.Document.AddSection();
                header = section.Headers.Primary.AddParagraph();
                header.AddText(string.Empty);

                var page = common.Pdf.AddPage();
                page.Orientation = PageOrientation.Landscape;
                page.Size = common.PdfTableHelper.PageSize;
                common.PdfPages.Add(page);
                section.AddPageBreak();
                table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);
            }

            if (request.Activities.Count % common.PdfTableHelper.ItemsPerPage > 0)
            {
                section.AddPageBreak();
            }

            var column = table.AddRow();
            column.Borders.Style = BorderStyle.None;

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Project");
            column.Cells[3].AddParagraph(request.ProjectName);

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Total Estimated:");
            column.Cells[3].AddParagraph(request.TotalEstimatedTime.ToString("0.00"));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Total Logged:");
            column.Cells[3].AddParagraph(request.TotalLoggedTime.ToString("0.00"));
            table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            table.Rows.Alignment = RowAlignment.Center;
            #endregion
        }

        private void PopulateForActivityListsByProjectReport(CommonPdfParameter common, ActivityListByProjectReportListViewModel request)
        {
            #region Header
            var section = common.Document.AddSection();
            var header = section.Headers.Primary.AddParagraph();
            header.AddText($"Project: {request.ProjectName}");
            header.Format.Font.Size = common.PdfTableHelper.HeaderSize;
            header.Format.Font.Color = Colors.DarkBlue;
            header.Format.Font.Bold = true;
            header.Format.Font.Italic = true;
            header.Format.Alignment = ParagraphAlignment.Center;
            #endregion

            #region Table

            var tableCells = common.HeaderNames;
            var table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            var iteration = 0;
            foreach (var activity in request.ActivityList)
            {
                var columnOrder = 0;
                var row = table.AddRow();
                row.Cells[columnOrder++].AddParagraph((++iteration).ToString());
                row.Cells[columnOrder++].AddParagraph(activity.ActivityListName);

                foreach (var activityType in activity.ActivityTypes)
                {
                    row.Cells[columnOrder++].AddParagraph(activityType?.Progress?.ToString() ?? "-");
                }

                if (activity.Average.HasValue)
                {
                    if (activity.Average == 100)
                    {
                        row.Cells[columnOrder++].AddParagraph("Completed");
                    }
                    else row.Cells[columnOrder++].AddParagraph(activity.Average == 0 ? "New" : "OnGoing");
                }
                else
                {
                    row.Cells[columnOrder++].AddParagraph("New");
                }

                row.Cells[columnOrder++].AddParagraph(activity.ActualDate.HasValue
                    ? $"{activity.ActualDate:yyyy-M-d}"
                    : "-");

                row.Cells[columnOrder++].AddParagraph(activity.PlannedDate.HasValue
                    ? $"{activity.PlannedDate:yyyy-M-d}"
                    : "-");

                if (iteration % common.PdfTableHelper.ItemsPerPage != 0) continue;

                section = common.Document.AddSection();
                header = section.Headers.Primary.AddParagraph();
                header.AddText(string.Empty);

                var page = common.Pdf.AddPage();
                page.Orientation = PageOrientation.Landscape;
                page.Size = common.PdfTableHelper.PageSize;
                common.PdfPages.Add(page);
                section.AddPageBreak();
                table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);
            }

            if (request.ActivityList.Count % common.PdfTableHelper.ItemsPerPage > 0)
            {
                section.AddPageBreak();
            }

            table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            table.Rows.Alignment = RowAlignment.Center;
            #endregion
        }

        private void PopulateForGetEmployeeLoggedTime(CommonPdfParameter common, UserLoggedTimeListViewModel request)
        {
            #region Header
            var section = common.Document.AddSection();
            var header = section.Headers.Primary.AddParagraph();
            header.AddText($"{request.UserName}");
            header.Format.Font.Size = common.PdfTableHelper.HeaderSize;
            header.Format.Font.Color = Colors.DarkBlue;
            header.Format.Font.Bold = true;
            header.Format.Font.Italic = true;
            header.Format.Alignment = ParagraphAlignment.Center;
            #endregion

            #region Table

            var tableCells = common.HeaderNames;
            var table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            var iteration = 0;

            foreach (var loggedTime in request.LoggedTimes)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph((++iteration).ToString());
                row.Cells[1].AddParagraph(loggedTime.ProjectName);
                row.Cells[2].AddParagraph(loggedTime.ActivityName);
                row.Cells[3].AddParagraph(loggedTime.TrackerName);
                row.Cells[4].AddParagraph(loggedTime.Time.ToString("0.00"));
                row.Cells[5].AddParagraph(string.Format("{0:yyyy-M-d}", loggedTime.DateOfWork));

                if (iteration % common.PdfTableHelper.ItemsPerPage != 0) continue;

                section = common.Document.AddSection();
                header = section.Headers.Primary.AddParagraph();
                header.AddText(string.Empty);

                var page = common.Pdf.AddPage();
                page.Orientation = PageOrientation.Landscape;
                page.Size = common.PdfTableHelper.PageSize;
                common.PdfPages.Add(page);
                section.AddPageBreak();
                table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);
            }

            if (request.LoggedTimes.Count % common.PdfTableHelper.ItemsPerPage > 0)
            {
                section.AddPageBreak();
            }

            var column = table.AddRow();
            column.Borders.Style = BorderStyle.None;

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Employee");
            column.Cells[3].AddParagraph(request.UserName);

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Start Date");
            column.Cells[3].AddParagraph(string.Format("{0:yyyy-M-d}", common.StartDate));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Due Date");
            column.Cells[3].AddParagraph(string.Format("{0:yyyy-M-d}", common.DueDate));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Total Estimated:");
            column.Cells[3].AddParagraph(request.TotalEstimatedTime.ToString("0.00"));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Total Logged:");
            column.Cells[3].AddParagraph(request.TotalLoggedTime.ToString("0.00"));
            table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            table.Rows.Alignment = RowAlignment.Center;
            #endregion
        }

        private void PopulateForGetEmployeesLoggedTimeByPeriod(CommonPdfParameter common, LoggedTimeByPeriodListViewModel request)
        {
            #region Header
            var section = common.Document.AddSection();
            var header = section.Headers.Primary.AddParagraph();
            header.Format.Font.Size = common.PdfTableHelper.HeaderSize;
            header.Format.Font.Color = Colors.DarkBlue;
            header.Format.Font.Bold = true;
            header.Format.Font.Italic = true;
            header.Format.Alignment = ParagraphAlignment.Center;
            #endregion

            #region Table

            var tableCells = common.HeaderNames;
            var table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            var iteration = 0;

            foreach (var user in request.UsersLoggedEstimatedTime)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph((++iteration).ToString());
                row.Cells[1].AddParagraph(user.Name);
                row.Cells[2].AddParagraph(user.UserEstimatedTimeByPeriod.ToString("0.00"));
                row.Cells[3].AddParagraph(user.UserLoggedTimeByPeriod.ToString("0.00"));

                if (iteration % common.PdfTableHelper.ItemsPerPage == 0)
                {
                    section = common.Document.AddSection();
                    header = section.Headers.Primary.AddParagraph();
                    header.AddText(string.Empty);

                    var page = common.Pdf.AddPage();
                    page.Orientation = PageOrientation.Landscape;
                    page.Size = common.PdfTableHelper.PageSize;
                    common.PdfPages.Add(page);
                    section.AddPageBreak();
                    table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);
                }
            }

            if (request.UsersLoggedEstimatedTime.Count % common.PdfTableHelper.ItemsPerPage > 0)
            {
                section.AddPageBreak();
            }

            var column = table.AddRow();
            column.Borders.Style = BorderStyle.None;

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Start Date");
            column.Cells[3].AddParagraph(string.Format("{0:yyyy-M-d}", common.StartDate));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Due Date");
            column.Cells[3].AddParagraph(string.Format("{0:yyyy-M-d}", common.DueDate));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Total Estimated:");
            column.Cells[3].AddParagraph(request.TotalEstimatedTime.ToString("0.00"));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Total Logged:");
            column.Cells[3].AddParagraph(request.TotalLoggedTime.ToString("0.00"));
            table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            table.Rows.Alignment = RowAlignment.Center;
            #endregion
        }

        private void PopulateForProjectGroupsGeneralReportListViewModel(CommonPdfParameter common,
            ProjectGroupsGeneralReportListViewModel request)
        {
            #region Header
            var section = common.Document.AddSection();
            var header = section.Headers.Primary.AddParagraph();
            header.Format.Font.Size = common.PdfTableHelper.HeaderSize;
            header.Format.Font.Color = Colors.DarkBlue;
            header.Format.Font.Bold = true;
            header.Format.Font.Italic = true;
            header.Format.Alignment = ParagraphAlignment.Center;
            #endregion

            #region Table

            var tableCells = common.HeaderNames;
            var table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            var totalRows = 0;
            foreach (var projectGroup in request.ProjectGroups)
            {
                var iteration = 0;

                foreach (var project in projectGroup.Projects)
                {
                    var row = table.AddRow();
                    row.Cells[0].AddParagraph((++iteration).ToString());
                    row.Cells[1].AddParagraph(projectGroup.ProjectGroupName);
                    row.Cells[2].AddParagraph(project.ProjectName);
                    row.Cells[3].AddParagraph(project.EstimatedTime.ToString("0.00"));
                    row.Cells[4].AddParagraph(project.LoggedTime.ToString("0.00"));

                    if (iteration % common.PdfTableHelper.ItemsPerPage == 0)
                    {
                        section = common.Document.AddSection();
                        header = section.Headers.Primary.AddParagraph();
                        header.AddText(string.Empty);

                        var page = common.Pdf.AddPage();
                        page.Orientation = PageOrientation.Landscape;
                        page.Size = common.PdfTableHelper.PageSize;
                        common.PdfPages.Add(page);
                        section.AddPageBreak();
                        table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);
                    }
                }

                var breakRow = table.AddRow();
                totalRows = iteration;
            }

            if (totalRows % common.PdfTableHelper.ItemsPerPage > 0)
            {
                section.AddPageBreak();
            }

            var column = table.AddRow();
            column.Borders.Style = BorderStyle.None;

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Start date");
            column.Cells[3].AddParagraph(string.Format("{0:yyyy-M-d}", common.StartDate));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Due date");
            column.Cells[3].AddParagraph(string.Format("{0:yyyy-M-d}", common.DueDate));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Total Estimated:");
            column.Cells[3].AddParagraph(request.TotalEstimatedTine.ToString("0.00"));

            column = table.AddRow();
            column.Format.Font.Bold = true;
            column.Cells[2].AddParagraph("Total Logged:");
            column.Cells[3].AddParagraph(request.TotalLoggedTime.ToString("0.00"));
            table = CreateTableHeader(ref section, common.PdfTableHelper, tableCells);

            table.Rows.Alignment = RowAlignment.Center;
            #endregion
        }
        #endregion

        public byte[] GetActivityListsByProjectReport(ActivityListByProjectReportListViewModel request)
        {
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            page.Orientation = PageOrientation.Landscape;
            page.Size = PageSize.A3;

            var pages = new List<PdfPage> { page };

            //Create pdf content
            var doc = CreateDocument(nameof(this.GetProjectGeneralReport),
                string.Format("{1}, {0}", "Default", "Default"));

            var commonParameter = new CommonPdfParameter
            {
                Document = doc,
                Pdf = pdf,
                PdfPages = pages
            };
            PopulateForActivityListsByProjectReport(commonParameter, request);

            //Create renderer for content
            var renderer = new DocumentRenderer(doc);
            renderer.PrepareDocument();

            var pageSize = GetPageSizeInXRectUnits(page.Size);

            var pageNumber = 0;
            foreach (var item in pages)
            {
                var gfx = XGraphics.FromPdfPage(item);
                var container = gfx.BeginContainer(pageSize, pageSize, XGraphicsUnit.Point);
                gfx.DrawRectangle(XPens.LightGray, pageSize);
                renderer.RenderPage(gfx, ++pageNumber);
                gfx.EndContainer(container);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                pdf.Save(ms, true);
                return ms.ToArray();
            }
        }

        public byte[] GetSprintListGeneralReport(SprintListGeneralReportListViewModel request, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }

        public byte[] GetProjectsByFiltersReport(ProjectFilteredReportListViewModel request, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }

        public byte[] GetProjectGroupsFilteredReportWithHistory(ProjectGroupFilteredReportListViewModel request, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }

        public byte[] GetTeamsByFiltersReport(TeamsFilteredReportListViewModel request, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }

        public byte[] GetEmployeeFilteredReport(EmployeeFilteredReportListViewModel request, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }

        public byte[] GetOverdueTasksFilteredReport(OverdueTasksFilteredReportViewModel request, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }

        public byte[] GetProjectGroupsWithHistoryFilteredReport(ProjectGroupFilteredReportListViewModel request, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }

        public byte[] GetEmployeeLoggedTime(UserLoggedTimeListViewModel request, DateTime startDate, DateTime dueDate)
        {
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            page.Orientation = PageOrientation.Landscape;
            page.Size = PageSize.A4;

            var pages = new List<PdfPage> { page };

            //Create pdf content
            var doc = CreateDocument(nameof(this.GetProjectGeneralReport),
                string.Format("{1}, {0}", "Default", "Default"));

            var commonParameter = new CommonPdfParameter
            {
                Document = doc,
                Pdf = pdf,
                PdfPages = pages,
                StartDate = startDate,
                DueDate = dueDate,
                PdfTableHelper = new PdfTableHelperA4()
            };
            PopulateForGetEmployeeLoggedTime(commonParameter, request);

            //Create renderer for content
            var renderer = new DocumentRenderer(doc);
            renderer.PrepareDocument();

            var pageSize = GetPageSizeInXRectUnits(page.Size);

            var pageNumber = 0;
            foreach (var item in pages)
            {
                var gfx = XGraphics.FromPdfPage(item);
                var container = gfx.BeginContainer(pageSize, pageSize, XGraphicsUnit.Point);
                gfx.DrawRectangle(XPens.LightGray, pageSize);
                renderer.RenderPage(gfx, ++pageNumber);
                gfx.EndContainer(container);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                pdf.Save(ms, true);
                return ms.ToArray();
            }
        }

        public byte[] GetEmployeesLoggedTimeByPeriod(LoggedTimeByPeriodListViewModel request, DateTime startDate,
            DateTime dueDate)
        {
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            page.Orientation = PageOrientation.Landscape;
            page.Size = PageSize.A4;

            var pages = new List<PdfPage> { page };

            //Create pdf content
            var doc = CreateDocument(nameof(this.GetProjectGeneralReport),
                string.Format("{1}, {0}", "Default", "Default"));

            var commonParameter = new CommonPdfParameter
            {
                Document = doc, Pdf = pdf, PdfPages = pages, StartDate = startDate,
                DueDate = dueDate, PdfTableHelper = new PdfTableHelperA4()
            };
            PopulateForGetEmployeesLoggedTimeByPeriod(commonParameter, request);

            //Create renderer for content
            var renderer = new DocumentRenderer(doc);
            renderer.PrepareDocument();

            var pageSize = GetPageSizeInXRectUnits(page.Size);

            var pageNumber = 0;
            foreach (var item in pages)
            {
                var gfx = XGraphics.FromPdfPage(item);
                var container = gfx.BeginContainer(pageSize, pageSize, XGraphicsUnit.Point);
                gfx.DrawRectangle(XPens.LightGray, pageSize);
                renderer.RenderPage(gfx, ++pageNumber);
                gfx.EndContainer(container);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                pdf.Save(ms, true);
                return ms.ToArray();
            }
        }

        public byte[] GetProjectGeneralReport(ProjectGeneralReportListViewModel request, DateTime startDate,
            DateTime dueDate)
        {
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            page.Orientation = PageOrientation.Landscape;
            page.Size = PageSize.A3;

            var pages = new List<PdfPage> { page };

            //Create pdf content
            var doc = CreateDocument(nameof(this.GetProjectGeneralReport),
                string.Format("{1}, {0}", "Default", "Default"));

            var commonParameter = new CommonPdfParameter
            {
                Document = doc, Pdf = pdf, PdfPages = pages, StartDate = startDate, DueDate = dueDate
            };
            PopulateForGeneralProjectReport(commonParameter, request);

            //Create renderer for content
            var renderer = new DocumentRenderer(doc);
            renderer.PrepareDocument();

            var pageSize = GetPageSizeInXRectUnits(page.Size);

            var pageNumber = 0;
            foreach (var item in pages)
            {
                var gfx = XGraphics.FromPdfPage(item);
                var container = gfx.BeginContainer(pageSize, pageSize, XGraphicsUnit.Point);
                gfx.DrawRectangle(XPens.LightGray, pageSize);
                renderer.RenderPage(gfx, ++pageNumber);
                gfx.EndContainer(container);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                pdf.Save(ms, true);
                return ms.ToArray();
            }
        }

        public byte[] GetProjectGroupsListReport(ProjectGroupsGeneralReportListViewModel request, DateTime startDate,
            DateTime dueDate)
        {
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            page.Orientation = PageOrientation.Landscape;
            page.Size = PageSize.A4;

            var pages = new List<PdfPage> { page };

            //Create pdf content
            var doc = CreateDocument(nameof(this.GetProjectGeneralReport),
                string.Format("{1}, {0}", "Default", "Default"));

            var commonParameter = new CommonPdfParameter
            {
                Document = doc, Pdf = pdf, PdfPages = pages, StartDate = startDate, DueDate = dueDate
            };
            PopulateForProjectGroupsGeneralReportListViewModel(commonParameter, request);
            
            //Create renderer for content
            var renderer = new DocumentRenderer(doc);
            renderer.PrepareDocument();

            var pageSize = GetPageSizeInXRectUnits(page.Size);

            var pageNumber = 0;
            foreach (var item in pages)
            {
                var gfx = XGraphics.FromPdfPage(item);
                var container = gfx.BeginContainer(pageSize, pageSize, XGraphicsUnit.Point);
                gfx.DrawRectangle(XPens.LightGray, pageSize);
                renderer.RenderPage(gfx, ++pageNumber);
                gfx.EndContainer(container);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                pdf.Save(ms, true);
                return ms.ToArray();
            }
        }

        public byte[] GetProjectGroupsFilteredReportWithHistory(ProjectGroupFilteredReportListViewModel request, DateTime startDate, DateTime dueDate, Interval intervalType, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }

        public byte[] GetGeneralFilteredReport(GeneralFilteredReportListViewModel request, IList<PdfTableCell> tableHeaders)
        {
            throw new NotImplementedException();
        }
    }
}
        
