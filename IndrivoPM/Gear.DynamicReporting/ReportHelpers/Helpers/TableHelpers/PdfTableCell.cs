using MigraDoc.DocumentObjectModel;

namespace Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers
{
    /// <summary>
    /// TableCell contains main features for the cell of the table.
    /// Its purpose is to be applied on table headers, because their
    /// size and styles must be predefined. The alignment of the
    /// paragraph is by default left one (ParagraphAlignment.Left).
    /// </summary>
    public class PdfTableCell
    {
        public string Name { get; set; }

        /// <summary>
        /// Should be defined in cm
        /// Example: "3,5cm"
        /// </summary>
        public string Width { get; set; }

        public ParagraphAlignment Alignment { get; set; } = ParagraphAlignment.Left;
    }
}
