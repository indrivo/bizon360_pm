using System;
using System.Collections.Generic;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers;
using Gear.DynamicReporting.Abstractions.ReportHelpers.Interfaces;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;

namespace Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers
{
    /// <summary>
    /// It has three required fields: Document, Pdf and PdfPages.
    /// The PdfTableHelper is by default dedicated for A3 document format.
    /// </summary>
    public class CommonPdfParameter
    {
        public Document Document { get; set; }

        public PdfDocument Pdf { get; set; }

        public List<PdfPage> PdfPages { get; set; }

        public IPdfTableHelperStyles PdfTableHelper { get; set; }

        public List<PdfTableCell> HeaderNames { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
