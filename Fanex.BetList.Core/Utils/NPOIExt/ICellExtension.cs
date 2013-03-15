// ---------------------------------------------------------------------------------
// <copyright file="ICellExtension.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.10">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Utils.NPOIExt
{
    using NPOI.SS.UserModel;

    public static class ICellExtension
    {
        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <param name="value">The value.</param>
        public static void SetCellValue(this ICell cell, object value)
        {
            if (value == null)
                cell.SetCellValue("0");
            else
            {
                if (value is string)
                    cell.SetCellValue((string)value);
                else if (value is int)
                    cell.SetCellValue((int)value);
                else if (value is long)
                    cell.SetCellValue((long)value);
                else if (value is double)
                    cell.SetCellValue((double)value);
                else if (value is decimal)
                    cell.SetCellValue((decimal)value);
                else if (value is bool)
                    cell.SetCellValue((bool)value);
                else if (value is System.DateTime)
                    cell.SetCellValue((System.DateTime)value);
                else if (value is IRichTextString)
                    cell.SetCellValue((IRichTextString)value);
                else
                    cell.SetCellValue("Undefined");
            }
        }
    }
}