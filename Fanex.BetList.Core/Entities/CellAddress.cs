// ---------------------------------------------------------------------------------
// <copyright file="CellAddress.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.17">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Entities
{
    public struct CellAddress
    {
        public CellAddress(int rowIndex, int columnIndex) : this()
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
        }

        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }
    }
}