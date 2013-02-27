namespace Sunplus.Agent.UI.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Reports;

    /// <summary>
    /// Summary description for WLByDateCommand
    /// </summary>
    public class WLByDateCmd : BetListCmd
    {
        private BetListBO betList = null;
        private int custId;
        private ICollection<KeyValuePair<string, bool>> filterParams = null;
        private DataTable outDataTable = null;
        private DataTable result = null;
        private DateTime fdate, tdate;
        private string userName = string.Empty;

        public WLByDateCmd(string sessionId, ICollection<KeyValuePair<string, bool>> permission)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
            this.outDataTable = new DataTable();
            this.filterParams = permission;
        }

        public override object Execute()
        {
            this.result = this.betList.GetWinLossByDateAndMem(this.custId, this.fdate, this.tdate, ReportsBO.MoveBetsDate, this.filterParams, out this.outDataTable);
            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return this.outDataTable;
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.custId = (int)hash["custid"];
            this.fdate = (DateTime)hash["fdate"];
            this.tdate = (DateTime)hash["tdate"];
            this.userName = hash["username"].ToString();

            if (hash["DateIsNull"] != null)
            {
                Title = Resources.BetList.lblMemberBetList;
            }
            else
            {
                Title = string.Join(null, new string[] { Resources.BetList.lblMemberBetList, " ", this.fdate.ToShortDateString(), " --> ", this.tdate.ToShortDateString(), "- ", this.userName });
            }

            BetListType = BetListType.Winlost;
        }
    }
}