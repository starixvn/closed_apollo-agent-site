// ---------------------------------------------------------------------------------
// <copyright file="BLDailySTCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.19">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListCmd
{
    using System;
    using System.Data;
    using Fanex.BetList.Age.Definition;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Reports;

    /// <summary>
    /// Summary description for DailyStatement
    /// </summary>
    public class BLDailySTCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        private int custId;
        private DateTime fdate;
        private int transType;
        private string userName = string.Empty;

        public BLDailySTCmd(string sessionId)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
            this.outDataTable = new DataTable();
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.custId = (int)hash["custid"];
            this.fdate = (DateTime)hash["fdate"];
            this.transType = (int)hash["transtype"];
            this.userName = hash["username"].ToString();

            Title = string.Join(null, new string[] { Resources.Lang.dailystatement, " ", this.fdate.ToShortDateString(), "- ", this.userName });

            this.BetListType = BetListType.Winlost | BetListType.DailyStatement;
        }

        public override object Execute()
        {
            if (this.fdate < ReportsBO.BalanceUpDate)
            {
                this.result = this.betList.GetDailyStatementOld(this.custId, this.fdate, out this.outDataTable);
            }
            else
            {
                this.result = this.betList.GetDailyStatement(this.custId, this.fdate, this.transType, out this.outDataTable);
            }

            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return this.outDataTable;
        }
    }
}