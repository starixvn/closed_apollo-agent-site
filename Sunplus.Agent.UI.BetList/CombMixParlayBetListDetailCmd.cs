namespace Sunplus.Agent.UI.BetList
{
    using System.Data;
    using Fanex.Data.DNAide;
    using Sunplus.Agent.Model.BL.BetList;

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

        public override void SetParams(System.Collections.Hashtable hash)
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