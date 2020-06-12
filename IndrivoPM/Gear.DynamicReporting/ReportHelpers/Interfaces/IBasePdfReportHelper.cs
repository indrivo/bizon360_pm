using System.Collections.Generic;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp;
using PdfSharp.Drawing;

namespace Gear.DynamicReporting.Abstractions.ReportHelpers.Interfaces
{
    public interface IBasePdfReportHelper
    {
        /// <summary>
        /// Creates table header with indicated styles.
        /// </summary>
        /// <param name="section">The section of the Document.</param>
        /// <param name="tableHelper">Consists all necessary styles.</param>
        /// <param name="tableCells">Sets dynamically the names of the table.</param>
        /// <returns>Table class</returns>
        Table CreateTableHeader(ref Section section, IPdfTableHelperStyles tableHelper, List<PdfTableCell> tableCells);

        /// <summary>
        /// It helps for converting content in actual byte[] of .pdf file.
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns>XRect structure</returns>
        XRect GetPageSizeInXRectUnits(PageSize pageSize);

        /// <summary>
        /// Creates a document with indicated title and subject.
        /// </summary>
        /// <param name="title">The title of the document.</param>
        /// <param name="subject">The subject of the document.</param>
        /// <returns>Document class</returns>
        Document CreateDocument(string title, string subject);
    }
}
