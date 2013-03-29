// ---------------------------------------------------------------------------------
// <copyright file="MixParlayCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.19">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListCmd
{
    using System.Collections;
    using System.Data;
    using Fanex.BetList.Age.Definition;
    using Sunplus.Agent.Model.BL.BetList;

    /// <summary>
    /// Summary description for MixParlayCmd
    /// Using to list Mix Parlay BetList
    /// </summary>
    public class MixParlayCmd : BetListCmd
    {
        private int bettype;
        private DataTable result = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        public MixParlayCmd()
        {
        }

        public MixParlayCmd(string sessionId)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
            this.outDataTable = new DataTable();
        }

        protected int Bettype
        {
            get { return this.bettype; }
            set { this.bettype = value; }
        }

        public override void SetParams(Hashtable hash)
        {
            this.bettype = (int)hash["bettype"];

            Title = Resources.Lang.lblMixParlay;
            BetListType = BetListType.Member;
        }

        public override object Execute()
        {
            this.result = this.betList.GetRunningByBettype(this.bettype);
            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return null;
        }
    }
}