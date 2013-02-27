using System;
using System.Collections.Generic;
using Fanex.Age.Utility.NPOI;

namespace Sunplus.Agent.UI.BetList.Header
{
    using BetTypes;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList.Excel;

    /// <summary>
    /// Default header for bet list.
    /// </summary>
    public class Header1 : Header1_Template, IBetItem
    {

        #region IBetItem Members

        public string Render()
        {
            return ToString();
        }

        public void Assign(int index, System.Data.DataRow row, BetListWorkbook workbook)
        {
        }

        public void Assign(int index, System.Data.DataRow row)
        {
            
        }

        public void CustomizeColumns(UserTypes userType, BetListType betListType, BetListWorkbook workbook)
        {
            CustomizeColumns(userType, betListType);

            var removeColumns = new List<int>();

            if ((betListType & BetListType.Canceled) == BetListType.Canceled)
            {
                removeColumns.Add(workbook.Columns.WinLoss);
                removeColumns.Add(workbook.Columns.Super);
                removeColumns.Add(workbook.Columns.Master);
                removeColumns.Add(workbook.Columns.Agent);
            }
            else
            {
                if ((betListType & BetListType.Winlost) != BetListType.Winlost)
                {
                    removeColumns.Add(workbook.Columns.WinLoss);
                }

                if ((betListType & BetListType.Member) != BetListType.Member)
                {
                    removeColumns.Add(workbook.Columns.Member);
                }

                if ((betListType & BetListType.DailyStatement) == BetListType.DailyStatement)
                {
                    removeColumns.Add(workbook.Columns.Super);
                    removeColumns.Add(workbook.Columns.Master);
                    removeColumns.Add(workbook.Columns.Agent);
                }

                switch (userType)
                {
                    case UserTypes.Super://super 
                        break;
                    case UserTypes.Master://master
                        removeColumns.Add(workbook.Columns.Super);
                        break;
                    case UserTypes.Agent://agent
                        removeColumns.Add(workbook.Columns.Super);
                        removeColumns.Add(workbook.Columns.Master);
                        break;
                    default: // Hide all position taking
                        removeColumns.Add(workbook.Columns.Super);
                        removeColumns.Add(workbook.Columns.Master);
                        removeColumns.Add(workbook.Columns.Agent);
                        break;

                }
            }

            workbook.RemoveColumns(removeColumns);

            var values = new List<object>() { "#" };
            if (this.Member != null)
            {
                values.Add(Resources.BetList.member);
            }
            values.AddRange(new List<object> { Resources.BetList.transtime, Resources.BetList.choice, Resources.BetList.odds, Resources.BetList.stake });
            if (this.Winloss != null)
            {
                values.Add(Resources.BetList.winloss);
            }
            values.Add(Resources.BetList.status);
            if (this.Super != null)
            {
                values.Add(Resources.BetList.superPTcomm.Replace("<br/>", "\n"));
            }
            if (this.Master != null)
            {
                values.Add(Resources.BetList.masterPTcomm.Replace("<br/>", "\n"));
            }
            if (this.Agent != null)
            {
                values.Add(Resources.BetList.agentPTcomm.Replace("<br/>", "\n"));
            }

            workbook.ResultSheet.GetRow(workbook.ColumnHeaderRowIndex).WriteCells(0, values);
        }

        public void CustomizeColumns(UserTypes userType, BetListType betListType)
        {

            if ((betListType & BetListType.Canceled) == BetListType.Canceled)
            {
                this.Winloss = null;
                this.Super = null;
                this.Master = null;
                this.Agent = null;
            }
            else
            {
                if ((betListType & BetListType.Winlost) != BetListType.Winlost)
                {
                    this.Winloss = null;
                }

                if ((betListType & BetListType.Member) != BetListType.Member)
                {
                    this.Member = null;
                }

                if ((betListType & BetListType.DailyStatement) == BetListType.DailyStatement)
                {
                    this.Super = null;
                    this.Master = null;
                    this.Agent = null;
                }

                switch (userType)
                {
                    case UserTypes.Super://super 
                        break;
                    case UserTypes.Master://master
                        this.Super = null;
                        break;
                    case UserTypes.Agent://agent
                        this.Super = null;
                        this.Master = null;
                        break;
                    default: // Hide all position taking
                        this.Super = null;
                        this.Master = null;
                        this.Agent = null;
                        break;

                }
            }
        }

        #endregion

        #region IComponent Members

        public void HideLinkedResources()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBetItem Members

        public List<int> ScoreMapBetTypes { get; set; }

        public bool IsAdminCanViewVNIP { get; set; }

        public bool ExportExcel { get; set; }

        public string Culture { get; set; }

        public System.Data.DataTable OutDataTable { get; set; }

        #endregion
    }
}
