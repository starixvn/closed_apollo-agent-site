namespace Sunplus.Agent.UI.BetList
{
    using System.Data;
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

        public override void SetParams(System.Collections.Hashtable hash)
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