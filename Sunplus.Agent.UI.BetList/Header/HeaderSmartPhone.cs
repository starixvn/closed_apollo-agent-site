namespace Sunplus.Agent.UI.BetList.HeaderSmartPhone
{
    using Sunplus.Agent.UI.BetList.BetTypes;
    using Sunplus.Agent.Model.BL.Definitions;
    using System;
    using Sunplus.Agent.UI.BetList.Excel;
    using System.Collections.Generic;

    public class HeaderSmartPhone : Sunplus.Agent.UI.BetList.Header.HeaderSmartPhone_Template, IBetItem
    {
        #region IBetItem Members

        public string Render()
        {
            return ToString();
        }

        public void Assign(int index, System.Data.DataRow row, BetListWorkbook workbook)
        {
            throw new NotImplementedException();
        }

        public void Assign(int index, System.Data.DataRow row)
        {

        }

        public void CustomizeColumns(UserTypes userType, BetListType betListType, BetListWorkbook workbook)
        {
            throw new NotImplementedException();
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
                switch (userType)
                {
                    case UserTypes.Super://super 
                        this.Master = null;
                        this.Agent = null;
                        break;
                    case UserTypes.Master://master
                        this.Super = null;
                        this.Agent = null;
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
