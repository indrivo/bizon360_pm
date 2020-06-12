using System.Collections.Generic;
using System.Linq;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;
using Gear.DynamicReporting.ProjectManagement.ReportHelpers.Interfaces;
using MigraDoc.DocumentObjectModel;
using PdfSharp;

namespace Gear.DynamicReporting.ProjectManagement.ReportHelpers.Helpers.PdfTableHelpers
{
    /// <summary>
    /// Table helper dedicated specifically to A4 documents.
    /// Contains predefined formats and number of items per list
    /// and a concrete number of headers with specified alignment,
    /// indentation, styles for each method.
    /// </summary>
    public class PdfTableHelperA4 : IPdfTableHelper
    {
        public int ItemsPerPage { get; set; } = 28;

        public PageSize PageSize { get; } = PageSize.A4;

        #region Header Properties
        public int HeaderSize { get; set; } = 25;

        public string TableStyle { get; set; } = "Table";

        public float BordersWidth { get; set; } = 0.2f;

        public float LargeBorderWidth { get; set; } = 0.5f;

        public float BordersLeftWidth { get; set; } = 0f;

        public float BordersRightWidth { get; set; } = 0f;

        public float RowsLeftIndent { get; set; } = 0f;
        #endregion

        public List<PdfTableCell> GetProjectGeneralReportTableHeaders()
        {
            return new List<PdfTableCell>
            {
                new PdfTableCell { Name = "#", Width = "1cm" },
                new PdfTableCell { Name = "Sprint", Width = "3cm" },
                new PdfTableCell { Name = "Employee", Width = "4cm" },
                new PdfTableCell { Name = "Activity", Width = "6,5cm" },
                new PdfTableCell { Name = "Priority", Width = "2cm" },
                new PdfTableCell { Name = "Est.", Width = "1cm", Alignment = ParagraphAlignment.Right },
                new PdfTableCell { Name = "Log.", Width = "1cm", Alignment = ParagraphAlignment.Right },
                new PdfTableCell { Name = "Progress", Width = "2cm", Alignment = ParagraphAlignment.Right },
                new PdfTableCell { Name = "Status", Width = "2cm", Alignment = ParagraphAlignment.Center },
                new PdfTableCell { Name = "Modified", Width = "3cm", Alignment = ParagraphAlignment.Center }
            };
        }

        public List<PdfTableCell> GetEmployeeLoggedTimeTableHeaders()
        {
            return new List<PdfTableCell>
            {
                new PdfTableCell { Name = "#", Width = "2cm", Alignment = ParagraphAlignment.Center },
                new PdfTableCell { Name = "Project", Width = "6cm" },
                new PdfTableCell { Name = "Activity", Width = "7cm" },
                new PdfTableCell { Name = "Tracker", Width = "6cm"},
                new PdfTableCell { Name = "Time", Width = "2cm" },
                new PdfTableCell { Name = "Date of work", Width = "3cm", Alignment = ParagraphAlignment.Right }
            };
        }

        public List<PdfTableCell> GetEmployeesLoggedTimeByPeriodTableHeaders()
        {
            return new List<PdfTableCell>
            {
                new PdfTableCell { Name = "#", Width = "2,5cm", Alignment = ParagraphAlignment.Center },
                new PdfTableCell { Name = "Project", Width = "7,5cm" },
                new PdfTableCell { Name = "Est.", Width = "3cm", Alignment = ParagraphAlignment.Right },
                new PdfTableCell { Name = "log.", Width = "3cm", Alignment = ParagraphAlignment.Right }
            };
        }

        public List<PdfTableCell> GetProjectGroupsListReportTableHeaders()
        {
            return new List<PdfTableCell>
            {
                new PdfTableCell { Name = "#", Width = "1cm" },
                new PdfTableCell { Name = "Project Group", Width = "6,5cm" },
                new PdfTableCell { Name = "Project", Width = "6,5cm" },
                new PdfTableCell { Name = "Est.", Width = "2cm", Alignment = ParagraphAlignment.Right },
                new PdfTableCell { Name = "Log.", Width = "2cm", Alignment = ParagraphAlignment.Right }
            };
        }

        public List<PdfTableCell> GetActivityListsByProjectReportTableHeaders(List<string> activityTypes)
        {
            var response = new List<PdfTableCell>
            {
                new PdfTableCell {Name = "#", Width = "1cm" },
                new PdfTableCell {Name = "Activity list", Width = "5,5cm" }
            };

            response.AddRange(activityTypes.Select(item => new PdfTableCell { Name = item + " %", Width = "1,5cm", Alignment = ParagraphAlignment.Center }));

            response.AddRange(new List<PdfTableCell>
            {
                new PdfTableCell { Name = "Status", Width = "2cm", Alignment = ParagraphAlignment.Right },
                new PdfTableCell { Name = "Planned date", Width = "2cm", Alignment = ParagraphAlignment.Right },
                new PdfTableCell { Name = "Actual date", Width = "2cm", Alignment = ParagraphAlignment.Right }
            });

            return response;
        }
    }
}
