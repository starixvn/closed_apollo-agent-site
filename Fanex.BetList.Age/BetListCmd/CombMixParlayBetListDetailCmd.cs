// ---------------------------------------------------------------------------------
// <copyright file="CombMixParlayBetListDetailCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.19">Create</change>
// </history>
// ---------------------------------------------------------------------------------

using System.Collections;
using System.Data;
using Fanex.Data.DNAide;
using Sunplus.Agent.Model.BL.BetList;

namespace Fanex.BetList.Age.BetListCmd
{
    /// <summary>
    /// Summary description for Combination MixParlayBetList Detail Command
    /// </summary>
    public class CombMixParlayBetListDetailCmd : BetListCmd
    {
        private DbResult result = null;
        private BetListBO betList = null;

        private long refNo = 0;
        private long transId = 0;
        private long betId = 0;
        private int custId = 0;

        public CombMixParlayBetListDetailCmd(string sessionId)
        {
            this.betList = new BetListBO(sessionId);
        }

        public override void SetParams(Hashtable hash)
        {
            this.refNo = (long)hash["refno"];
            this.transId = (long)hash["transid"];
            this.betId = (long)hash["betid"];
            this.custId = (int)hash["custid"];
        }

        public override object Execute()
        {
            this.result = this.betList.GetCombinationMixParlayDetail(this.refNo, this.transId, this.betId, this.custId);
            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return null;
        }
    }
}