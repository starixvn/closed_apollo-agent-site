namespace Sunplus.Agent.UI.BetList.Footer
{
    using System;
    using System.Collections.Generic;
    using Sunplus.Agent.UI.BetList.Utils;
    using Fanex.Age.Utility.NPOI;
    using BetTypes;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList.Excel;

    /// <summary>
    /// Total row for bet list
    /// </summary>
    public class Footer1 : Footer1_Template, IBetItem
    {
        int subTotalColumns = 6;
        int statusColumns = 4;

        #region IBetItem Members

        public string Render()
        {
            throw new NotImplementedException();
        }

        public void Assign(int index, System.Data.DataRow row, BetListWorkbook workbook)
        {
            Assign(index, row);

            var render = new RtfTextRender(workbook.NormalFont);
            var numberTextRender = new NumberTextRender(render, workbook.PosFont, workbook.NegFont);

            int rowId = workbook.FooterRowIndex;
            workbook.ResultSheet.CopyRows(workbook.TemplateSheet, 5, rowId, 3);

            workbook.ResultSheet.GetRow(rowId).GetCell(0).SetCellValue(Resources.BetList.subtotalWinlost + ":");
            numberTextRender.Render(this.Winloss.subTotalWinloss);
            workbook.ResultSheet.GetRow(rowId).GetCell(workbook.Columns.WinLoss).SetCellValue(render.Render());
            render.Clear();

            workbook.ResultSheet.GetRow(rowId + 1).GetCell(0).SetCellValue(Resources.BetList.subtotalCom + ":");
            workbook.ResultSheet.GetRow(rowId + 1).GetCell(workbook.Columns.WinLoss).SetCellValue(this.Winloss.subTotalCom);

            workbook.ResultSheet.GetRow(rowId + 2).GetCell(0).SetCellValue(Resources.BetList.total + ":");
            numberTextRender.Render(this.Winloss.total);
            workbook.ResultSheet.GetRow(rowId + 2).GetCell(workbook.Columns.WinLoss).SetCellValue(render.Render());
            render.Clear();
        }

        public void Assign(int index, System.Data.DataRow row)
        {
            if (row.Table.Columns.Contains("winlost") && row.Table.Columns.Contains("playercomm"))
            {
                string winlost = ConvertByBetType.Winloss(row["winlost"]);
                string playercomm = ConvertByBetType.Comm(row["playercomm"]);
                decimal temp = (decimal.Parse(row["winlost"].ToString()) + decimal.Parse(row["playercomm"].ToString()));

                this.Winloss.subTotalWinloss = winlost;
                this.Winloss.subTotalCom = playercomm;
                this.Winloss.total = ConvertByBetType.Winloss(temp);
            }
            else
            {
                this.Winloss.subTotalCom = "0";
                this.Winloss.subTotalWinloss = "0";
                this.Winloss.total = "0";
            }
        }

        public void CustomizeColumns(UserTypes userType, BetListType betListType, BetListWorkbook workbook)
        {
            CustomizeColumns(userType, betListType);
        }

        public void CustomizeColumns(UserTypes userType, BetListType betListType)
        {
            if ((betListType & BetListType.Canceled) == BetListType.Canceled)
            {
                this.Status = null;
                this.subTotalColSpan = subTotalColumns.ToString();
            }
            else
            {
                if ((betListType & BetListType.Member) != BetListType.Member)
                {
                    subTotalColumns = subTotalColumns - 1;
                }

                this.subTotalColSpan = subTotalColumns.ToString();

                if ((betListType & BetListType.DailyStatement) == BetListType.DailyStatement)
                {
                    statusColumns = statusColumns - 3;
                }

                switch (userType)
                {
                    case UserTypes.Super://super 
                        break;
                    case UserTypes.Master://master
                        statusColumns = statusColumns - 1;
                        break;
                    case UserTypes.Agent://agent
                        statusColumns = statusColumns - 2;
                        break;
                    default: // Hide all position taking
                        statusColumns = statusColumns - 3;
                        break;
                }

                this.Status.statusColSpan = statusColumns.ToString();
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
