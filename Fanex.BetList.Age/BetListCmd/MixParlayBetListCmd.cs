// ---------------------------------------------------------------------------------
// <copyright file="MixParlayBetListCmd.cs" company="Nexcel Solutions Vietnam">
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
    /// Summary description for MixParlayBetListCmd
    /// </summary>
    public class MixParlayBetListCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;
        private long transId;

        public MixParlayBetListCmd()
        {
        }

        public MixParlayBetListCmd(string sessionId)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
        }

        protected DataTable Result
        {
            get { return this.result; }
            set { this.result = value; }
        }

        protected BetListBO BetList
        {
            get { return this.betList; }
            set { this.betList = value; }
        }

        protected long TransId
        {
            get { return this.transId; }
            set { this.transId = value; }
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {
            this.transId = Convert.ToInt64(hash["transid"]);

            this.BetListType = BetListType.None;
        }

        public override object Execute()
        {
            this.result = this.betList.GetMixParlayDetail(this.transId);
            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return null;
        }
    }
}