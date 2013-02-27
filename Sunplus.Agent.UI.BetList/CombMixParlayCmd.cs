namespace Sunplus.Agent.UI.BetList
{
    /// <summary>
    /// Summary description for CombinationMixParlayCmd
    /// Using to list Combination Mix Parlay BetList
    /// </summary>
    public class CombinationMixParlayCmd : MixParlayCmd
    {
        public CombinationMixParlayCmd(string sessionId) : base(sessionId)
        {
        }   
  
        public override void SetParams(System.Collections.Hashtable hash)
        {
            Bettype = (int)hash["bettype"];
            Title = Resources.Lang.lblSystemParlay;         
            BetListType = BetListType.Member;
        }     
    }
}