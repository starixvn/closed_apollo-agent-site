// ---------------------------------------------------------------------------------
// <copyright file="MixParlayByMatchCmd.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Summary description for MixParlayByMatchCmd
    /// </summary>
    public class MixParlayByMatchCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        private DateTime fdate, tdate;
        private int betType = 0;

        public MixParlayByMatchCmd(string sessionId)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
            this.outDataTable = new DataTable();
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.fdate = (DateTime)hash["fdate"];
            this.tdate = (DateTime)hash["tdate"];
            this.betType = (int)hash["bettype"];

            if (hash["DateIsNull"] != null)
            {
                Title = Resources.BetList.lblMemberBetList;
            }
            else if (this.betType == 9)
            {
                Title = string.Join(null, new string[] { Resources.Lang.lblMixParlay, " ", this.fdate.ToShortDateString(), " --> ", this.tdate.ToShortDateString() });
            }
            else
            {
                Title = string.Join(null, new string[] { Resources.Lang.lblSystemParlay, " ", this.fdate.ToShortDateString(), " --> ", this.tdate.ToShortDateString() });
            }

            BetListType = BetListType.Member | BetListType.Winlost;
        }

        public override object Execute()
        {
            this.result = this.betList.GetParlayWinloss(this.fdate, this.tdate, this.betType);
            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return null;
        }
    }
}