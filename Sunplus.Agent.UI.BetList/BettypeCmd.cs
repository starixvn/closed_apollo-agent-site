namespace Sunplus.Agent.UI.BetList
{
    using System.Data;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    /// Summary description for BettypeCmd
    /// </summary>
    public class BettypeCmd : BetListCmd
    {
        private DataTable result = null;
        private BetListBO betList = null;

        private int betType = 0;
        
        public BettypeCmd(string sessionId)
        {
            this.result = new DataTable();
            this.betList = new BetListBO(sessionId);
            Title = Resources.BetList.correctscore;
        }

        public override void SetParams(System.Collections.Hashtable hash)
        {                                    
            this.betType = (int)hash["bettype"];
            Title = CacheRef.GetBettype(this.betType).ToUpper();
            this.BetListType = BetListType.Member;
        }

        public override object Execute()
        {          
            this.result = this.betList.GetRunningByBettype(this.betType);          
            return this.result;
        }

        public override DataTable OutDataTable()
        {
            return null;
        }
    }
}