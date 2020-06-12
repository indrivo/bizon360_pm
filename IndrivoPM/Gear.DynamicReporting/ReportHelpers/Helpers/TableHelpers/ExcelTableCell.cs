using OfficeOpenXml.Style;

namespace Gear.DynamicReporting.Abstractions.ReportHelpers.Helpers.TableHelpers
{
    /// <summary>
    /// ExcelTableCell contains main features for the cell of the table.
    /// Its purpose is to be applied on table headers in order to create
    /// dynamic report with option of filtering data (by including/excluding
    /// fields, change width of columns etc.). The alignment of the
    /// paragraph is by default left one (ExcelHorizontalAlignment.Left).
    /// </summary>
    public class ExcelTableCell
    {
        public string Name { get; }

        public bool IsActive { get; set; }

        public bool IsRemovable { get; set; }

        public int Width { get; set; }

        public int Order { get; set; }

        public ExcelHorizontalAlignment Alignment { get; set; } 
            = ExcelHorizontalAlignment.Left;

        public ExcelTableCell(string name, int order, bool isActive = true, bool isRemovable = true)
        {
            Name = name;
            Order = order;
            IsActive = isActive;
            IsRemovable = isRemovable;
        }
    }
}
