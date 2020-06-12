using System.Collections.Generic;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Gear.DynamicReporting.Abstractions.ReportHelpers.Interfaces
{
    /// <summary>
    /// Provides a base functionality for future use of .xlsx dynamic reports. Its methods have Xml keyword in their names,
    /// because the OfficeOpenXml library is used as reference link inside the project and it truly has xml implementation inside.
    /// </summary>
    public interface IBaseExcelReportHelper
    {
        #region Alignment

        /// <summary>
        /// Method used for setting left alignment for the specified list of columns.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="columns">The list which contains columns. Columns start from 1 (one).</param>
        void SetXmlColumnLeftHorizontalAlignment(ref ExcelWorksheet worksheet, List<int> columns);

        /// <summary>
        /// Method used for setting right alignment for the specified list of columns.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="columns">The list which contains columns. Columns start from 1 (one).</param>
        void SetXmlColumnRightHorizontalAlignment(ref ExcelWorksheet worksheet, List<int> columns);

        /// <summary>
        /// Method used for setting center alignment for the specified list of columns.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="columns">The list which contains columns. Columns start from 1 (one).</param>
        void SetXmlColumnCenterHorizontalAlignment(ref ExcelWorksheet worksheet, List<int> columns);

        #endregion

        #region Table Headers

        /// <summary>
        /// Sets table headers for the table dynamically, having the information of the starting row and column
        /// and the list of the header names.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The starting row.</param>
        /// <param name="startColumn">The starting column (Uppercase english alphabet character).</param>
        /// <param name="names">The list of total header names for the corresponding table.</param>
        void SetXmlTableHeader(ref ExcelWorksheet worksheet, int row, char startColumn, List<string> names);

        /// <summary>
        /// Sets table headers for the table dynamically, having the information of the starting row and column
        /// and the list of the header names.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The starting row.</param>
        /// <param name="startColumn">The starting column (Uppercase sequence of english alphabet characters).</param>
        /// <param name="names">The list of total header names for the corresponding table.</param>
        void SetXmlTableHeader(ref ExcelWorksheet worksheet, int row, string startColumn, List<string> names);

        #endregion

        #region Column Width

        /// <summary>
        /// Sets widths dynamically for each indicated columns in the list.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="startColumn">The starting column (natural number).</param>
        /// <param name="widths">the list of each width</param>
        void SetXmlColumnsWidth(ref ExcelWorksheet worksheet, int startColumn, List<int> widths);

        #endregion

        #region Column Bold style

        /// <summary>
        /// Makes each column of a specified row bold (applies corresponding style).
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row on each columns will be applied bold font.</param>
        /// <param name="startColumn">The column from which the method will start applying font style.</param>
        /// <param name="totalColumns">The total number of columns.</param>
        void SetXmlColumnsBold(ref ExcelWorksheet worksheet, int row, char startColumn, int totalColumns);

        /// <summary>
        /// Makes each column of a specified row bold (applies corresponding style).
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row on each columns will be applied bold font.</param>
        /// <param name="startColumn">The column from which the method will start applying font style (It represents
        /// a sequence of english alphabet uppercase characters).</param>
        /// <param name="totalColumns">The total number of columns.</param>
        void SetXmlColumnsBold(ref ExcelWorksheet worksheet, int row, string startColumn, int totalColumns);

        #endregion

        #region Top Border Style

        /// <summary>
        /// Draws the line above the indicated row (on top) and applies the corresponding border style.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row on each columns will be drawn the line.</param>
        /// <param name="startColumn">Indicates the starting column for line drawing (english alphabet uppercase character).</param>
        /// <param name="endColumn">Indicates the last column for drawing (inclusive). Is represented by english alphabet uppercase character</param>
        /// <param name="borderStyle">The style for the corresponding drawn line.</param>
        void SetXmlTopBorderStyle(ref ExcelWorksheet worksheet, int row,
            char startColumn, char endColumn, ExcelBorderStyle borderStyle);

        /// <summary>
        /// Draws the line above the indicated row (on top) and applies the corresponding border style.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row on each columns will be drawn the line.</param>
        /// <param name="startColumn">Indicates the starting column for line drawing (english alphabet uppercase character).</param>
        /// <param name="numberOfColumns">Indicates the total number of columns.</param>
        /// <param name="borderStyle">The style for the corresponding drawn line.</param>
        void SetXmlTopBorderStyle(ref ExcelWorksheet worksheet, int row,
            char startColumn, int numberOfColumns, ExcelBorderStyle borderStyle);

        /// <summary>
        /// Sets the top border line for the specified row on indicated range of columns.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row on each columns will be drawn the line.</param>
        /// <param name="startColumn">Indicates the starting column for line drawing (a sequence of english alphabet uppercase characters).</param>
        /// <param name="numberOfColumns">Indicates the number of columns on which the changes will be applied.</param>
        /// <param name="borderStyle">The style for the corresponding drawn line.</param>
        void SetXmlTopBorderStyle(ref ExcelWorksheet worksheet, int row,
            string startColumn, int numberOfColumns, ExcelBorderStyle borderStyle);

        #endregion

        #region Bottom Border Style

        /// <summary>
        /// Draws the row under the indicated row (on bottom) and applies the corresponding border style.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row on each columns will be drawn the line.</param>
        /// <param name="startColumn">Indicates the starting column for line drawing (english alphabet uppercase character).</param>
        /// <param name="endColumn">Indicates the last column for drawing (english alphabet uppercase character). 
        /// On the last column also will be applied the choosed border style.</param>
        /// <param name="borderStyle">The style for the corresponding drawn line.</param>
        void SetXmlBottomBorderStyle(ref ExcelWorksheet worksheet, int row,
            char startColumn, char endColumn, ExcelBorderStyle borderStyle);

        /// <summary>
        /// Sets the bottom border line for the specified row on indicated range of columns.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row on each columns will be drawn the line.</param>
        /// <param name="startColumn">Indicates the starting column for line drawing (english alphabet uppercase character).</param>
        /// <param name="numberOfColumns">Indicates the number of columns on which the changes will be applied.</param>
        /// <param name="borderStyle">The style for the corresponding drawn line.</param>
        void SetXmlBottomBorderStyle(ref ExcelWorksheet worksheet, int row,
            char startColumn, int numberOfColumns, ExcelBorderStyle borderStyle);

        /// <summary>
        /// Sets the bottom border line for the specified row on indicated range of columns.
        /// </summary>
        /// <param name="worksheet">The worksheet on which the changes will be applied.</param>
        /// <param name="row">The row on each columns will be drawn the line.</param>
        /// <param name="startColumn">Indicates the starting column for line drawing (a sequence of english alphabet uppercase characters).</param>
        /// <param name="numberOfColumns">Indicates the number of columns on which the changes will be applied.</param>
        /// <param name="borderStyle">The style for the corresponding drawn line.</param>
        void SetXmlBottomBorderStyle(ref ExcelWorksheet worksheet, int row,
            string startColumn, int numberOfColumns, ExcelBorderStyle borderStyle);

        #endregion
    }
}
