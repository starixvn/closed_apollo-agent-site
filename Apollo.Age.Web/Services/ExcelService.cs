// --------------------------------------------------------------------------
// <copyright file="ExcelService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.1.3">Created</change>
// </history>

namespace Apollo.Age.Web.Services
{
    using System;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExcelService
    {
        /// <summary>
        /// Create new Font.
        /// </summary>
        /// <param name="hssfworkbook">HSSF workbook.</param>
        /// <param name="fontColor">Color of font.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="isBold">Is Bold or no.</param>
        /// <param name="isItalic">Is Italic or no.</param>
        /// <returns>Return New font.</returns>
        public IFont CreateFont(HSSFWorkbook hssfworkbook, short fontColor = -1, short fontSize = -1, bool isBold = false, bool isItalic = false)
        {
            var newfont = hssfworkbook.CreateFont();
            if (fontColor != -1)
            {
                newfont.Color = fontColor;
            }

            if (fontSize != -1)
            {
                newfont.FontHeightInPoints = fontSize;
            }

            if (isBold)
            {
                newfont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.BOLD;
            }

            newfont.IsItalic = isItalic;

            return newfont;
        }

        /// <summary>
        /// Create new style.
        /// </summary>
        /// <param name="hssfworkbook">HSSF workbook.</param>
        /// <param name="backgroundColor">Back ground color.</param>
        /// <param name="align">Horizontal Alignment.</param>
        /// <param name="font">Font of style.</param>
        /// <param name="isBorder">Border or no.</param>
        /// <returns>New style.</returns>
        public ICellStyle CreateStyle(
            HSSFWorkbook hssfworkbook,
            short backgroundColor = -1,
            HorizontalAlignment align = HorizontalAlignment.LEFT,
            IFont font = null,
            bool isBorder = true)
        {
            ICellStyle style = hssfworkbook.CreateCellStyle();
            if (backgroundColor != -1)
            {
                style.FillForegroundColor = backgroundColor;
                style.FillPattern = FillPatternType.SOLID_FOREGROUND;
                style.FillBackgroundColor = backgroundColor;
            }

            style.Alignment = align;
            style.VerticalAlignment = VerticalAlignment.CENTER;
            if (font != null)
            {
                style.SetFont(font);
            }

            if (isBorder)
            {
                style.BorderTop = BorderStyle.THIN;
                style.BorderLeft = BorderStyle.THIN;
                style.BorderRight = BorderStyle.THIN;
                style.BorderBottom = BorderStyle.THIN;
            }

            return style;
        }

        /// <summary>
        /// Create new cell with format is number.
        /// </summary>
        /// <param name="row">Row need create cell.</param>
        /// <param name="columIndex">Index of column need create cell.</param>
        /// <param name="style">Style of cell.</param>
        /// <param name="numberFormat">Format of number.</param>
        /// <param name="value">Value in cell.</param>
        public void CreateNumberCell(IRow row, int columIndex, ICellStyle style, short numberFormat, double value)
        {
            var cell = row.CreateCell(columIndex);
            cell.CellStyle = style;
            cell.SetCellType(CellType.NUMERIC);
            cell.CellStyle.DataFormat = numberFormat;
            cell.SetCellValue(value);
        }

        /// <summary>
        /// Create new cell with format is string.
        /// </summary>
        /// <param name="row">Row need create cell.</param>
        /// <param name="columIndex">Index of Column need create cell.</param>
        /// <param name="style">Style of cell.</param>
        /// <param name="value">Value in cell.</param>
        public void CreateStringCell(IRow row, int columIndex, ICellStyle style, string value)
        {
            var cell = row.CreateCell(columIndex);
            cell.CellStyle = style;
            cell.SetCellValue(value);
        }

        /// <summary>
        /// Customize Color of workbook.
        /// </summary>
        /// <param name="hssfworkbook">Workbook need Customize.</param>
        public void CustomizeColor(HSSFWorkbook hssfworkbook)
        {
            var palette = hssfworkbook.GetCustomPalette();
            palette.SetColorAtIndex(HSSFColor.BLUE_GREY.index, (byte)141, (byte)200, (byte)245);
            palette.SetColorAtIndex(HSSFColor.BROWN.index, (byte)104, (byte)119, (byte)128);
            palette.SetColorAtIndex(HSSFColor.RED.index, (byte)153, (byte)0, (byte)0);

            palette.SetColorAtIndex(HSSFColor.DARK_RED.index, (byte)193, (byte)0, (byte)0);
            palette.SetColorAtIndex(HSSFColor.GREY_25_PERCENT.index, (byte)219, (byte)229, (byte)238);
            palette.SetColorAtIndex(HSSFColor.GREY_50_PERCENT.index, (byte)196, (byte)217, (byte)238);
            palette.SetColorAtIndex(HSSFColor.GREEN.index, (byte)0, (byte)128, (byte)129);
        }

        /// <summary>
        /// Get Date Format (year-month-day).
        /// </summary>
        /// <param name="dt">Object date need format.</param>
        /// <returns>Return string date.</returns>
        public string ShortDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:yyyy-MM-dd}", dt);
        }
    }
}