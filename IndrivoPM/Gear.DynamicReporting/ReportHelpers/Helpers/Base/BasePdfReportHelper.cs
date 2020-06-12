using System.Collections.Generic;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Interfaces;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp;
using PdfSharp.Drawing;

namespace Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.Base
{
    public class BasePdfReportHelper : IBasePdfReportHelper
    {
        /// <summary>
        /// Creates table header with indicated styles.
        /// </summary>
        /// <param name="section">The section of the Document.</param>
        /// <param name="tableHelper">Consists all necessary styles.</param>
        /// <param name="tableCells">Sets dynamically the names of the table.</param>
        /// <returns>Table class</returns>
        public Table CreateTableHeader(ref Section section, IPdfTableHelperStyles tableHelper, List<PdfTableCell> tableCells)
        {
            var table = section.AddTable();
            table.Style = tableHelper.TableStyle;
            table.Borders.Color = Colors.LightGray;
            table.Borders.Width = tableHelper.BordersWidth;
            table.Borders.Left.Width = tableHelper.BordersLeftWidth;
            table.Borders.Right.Width = tableHelper.BordersRightWidth;
            table.Rows.LeftIndent = tableHelper.RowsLeftIndent;

            foreach (var cell in tableCells)
            {
                var column = table.AddColumn(cell.Width);
                column.Format.Alignment = cell.Alignment;
            }

            var row = table.AddRow();
            row.Borders.Width = tableHelper.LargeBorderWidth;
            row.Borders.Color = Colors.Black;
            for (var i = 0; i < tableCells.Count; i++)
            {
                var paragraph = row.Cells[i];
                paragraph.Format.Font.Bold = true;
                row.Cells[i].AddParagraph(tableCells[i].Name);
            }

            return table;
        }

        /// <summary>
        /// It helps for converting content in actual byte[] of .pdf file.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns>XRect structure</returns>
        public XRect GetPageSizeInXRectUnits(PageSize pageSize)
        {
            switch (pageSize)
            {
                case PageSize.A0:
                    return new XRect(0, 0, XUnit.FromCentimeter(84.1).Point, XUnit.FromCentimeter(118.9).Point);
                case PageSize.A1:
                    return new XRect(0, 0, XUnit.FromCentimeter(59.4).Point, XUnit.FromCentimeter(84.1).Point);
                case PageSize.A2:
                    return new XRect(0, 0, XUnit.FromCentimeter(42).Point, XUnit.FromCentimeter(59.4).Point);
                case PageSize.A3:
                    return new XRect(0, 0, XUnit.FromCentimeter(29.7).Point, XUnit.FromCentimeter(42).Point);
                default: // A4
                    return new XRect(0, 0, XUnit.FromCentimeter(21).Point, XUnit.FromCentimeter(29.7).Point);
            }
        }

        /// <summary>
        /// Creates a document with indicated title and subject.
        /// </summary>
        /// <param name="title">The title of the document.</param>
        /// <param name="subject">The subject of the document.</param>
        /// <returns>Document class</returns>
        public Document CreateDocument(string title, string subject)
        {
            var document = new Document();

            #region DocInfo

            document.Info.Title = title;
            document.Info.Subject = subject;
            document.Info.Author = "Indrivo";

            #endregion

            #region DocStyle

            //Set normal text styles
            Style style = document.Styles[StyleNames.Normal];
            style.Font.Name = "Arial";

            //Set header styles
            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            //Set footer styles
            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            //Add table style
            style = document.Styles.AddStyle("Table", StyleNames.Normal);
            style.Font.Name = "Arial";
            style.Font.Size = 9;

            #endregion

            return document;
        }
    }
}
