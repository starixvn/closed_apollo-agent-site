// ---------------------------------------------------------------------------------
// <copyright file="IRowExtension.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.10">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Utils.NPOIExt
{
    using System.Collections.Generic;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Row extension interface class.
    /// </summary>
    public static class IRowExtension
    {
        /// <summary>
        /// Creates the cell.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="colId">The col id.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>Cell interface.</returns>
        public static ICell CreateCell(this IRow row, int colId, ICellStyle cellStyle)
        {
            ICell cell = row.CreateCell(colId);
            cell.CellStyle = cellStyle;
            return cell;
        }

        /// <summary>
        /// Writes the cells.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="colId">The col id.</param>
        /// <param name="values">The values.</param>
        public static void WriteCells(this IRow row, int colId, params object[] values)
        {
            int id = colId;
            if (values.Length == 1 && values[0] is List<object>)
                values = ((List<object>)values[0]).ToArray();

            foreach (var value in values)
            {
                row.GetCell(id).SetCellValue(value);
                id++;
            }
        }
    }
}