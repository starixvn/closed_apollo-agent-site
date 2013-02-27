namespace Sunplus.Agent.UI.BetList
{
    /// <summary>
    /// Summary description for Combination MixParlayBetList Command
    /// </summary>
    public class CombMixParlayBetListCmd : MixParlayBetListCmd
    {
        public CombMixParlayBetListCmd(string sessionId) : base(sessionId)
        {
        }

        public override object Execute()
        {
            Result = BetList.GetCombinationMixParlay(TransId);
            return Result;
        }     
    }
}