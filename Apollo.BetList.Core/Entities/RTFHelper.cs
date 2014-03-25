// ---------------------------------------------------------------------------------
// <copyright file="RTFHelper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.5">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Entities
{
    using Apollo.BetList.Core.Utils;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    public struct RTFHelper
    {
        public IWorkbook Workbook { get; set; }

        public RtfTextRender RTFRenderer { get; set; }

        /// <summary>
        /// Fonts the specified work book.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="boldWeight">The bold weight.</param>
        /// <param name="isTrike">If set to <c>true</c> [is trike].</param>
        /// <returns>
        /// Return the font.
        /// </returns>
        public IFont Font(short color, FontBoldWeight boldWeight = FontBoldWeight.Normal, bool isTrike = false)
        {
            IFont font = this.Workbook.CreateFont();
            font.FontName = "Tahoma";
            font.FontHeightInPoints = 9;
            font.IsStrikeout = isTrike;
            font.Color = color;
            font.Boldweight = (short)boldWeight;

            return font;
        }

        /// <summary>
        /// Cells the style.
        /// </summary>
        /// <param name="align">The align.</param>
        /// <param name="valign">The vertical align.</param>
        /// <param name="fontColor">Color of the font.</param>
        /// <param name="backgroundColor">Color of the background.</param>
        /// <param name="isBorder">If set to <c>true</c> [is border].</param>
        /// <returns>
        /// Return cell style.
        /// </returns>
        public ICellStyle CellStyle(
            HorizontalAlignment align, 
            VerticalAlignment valign, 
            short fontColor = HSSFColor.Black.Index, 
            short backgroundColor = HSSFColor.White.Index, 
            bool isBorder = false)
        {
            ICellStyle cellStyle = this.Workbook.CreateCellStyle();
            cellStyle.Alignment = align;
            cellStyle.VerticalAlignment = valign;
            cellStyle.FillForegroundColor = fontColor;
            cellStyle.FillBackgroundColor = backgroundColor;
            cellStyle.WrapText = true;

            if (isBorder)
            {
                cellStyle.BorderTop = cellStyle.BorderBottom = cellStyle.BorderRight = cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.TopBorderColor = cellStyle.BottomBorderColor = cellStyle.RightBorderColor = cellStyle.LeftBorderColor = HSSFColor.Black.Index;
            }

            return cellStyle;
        }
    }
}