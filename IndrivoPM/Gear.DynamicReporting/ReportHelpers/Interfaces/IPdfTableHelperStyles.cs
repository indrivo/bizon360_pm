using PdfSharp;

namespace Gear.DynamicReporting.Abstractions.ReportHelpers.Interfaces
{
    public interface IPdfTableHelperStyles
    {
        #region Table Properties
        int ItemsPerPage { get; set; }

        PageSize PageSize { get; }

        int HeaderSize { get; set; }

        string TableStyle { get; set; }

        float BordersWidth { get; set; }

        float LargeBorderWidth { get; set; }

        float BordersLeftWidth { get; set; }

        float BordersRightWidth { get; set; }

        float RowsLeftIndent { get; set; }

        #endregion
    }
}
