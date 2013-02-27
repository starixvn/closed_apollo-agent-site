namespace Sunplus.Agent.UI.BetList.FooterSmartPhone
{
    using System;
    using Sunplus.Agent.UI.BetList.BetTypes;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList.Utils;
    using Sunplus.Agent.UI.BetList.Excel;
    using System.Collections.Generic;

    public class FooterSmartPhone : Sunplus.Agent.UI.BetList.Footer.FooterSmartPhone_Template, IBetItem
    {
        int subTotalColumns = 5;
      
        #region IBetItem Members

        public string Render()
        {
            throw new NotImplementedException();
        }

        public void Assign(int index, System.Data.DataRow row, BetListWorkbook workbook)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
