using Sunplus.Agent.Utilities;


namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// 1st Handicap
    /// </summary>
    public class BetType7 : BetType1
    {
        public override void AssignFinalScore()
        {
            FinalScore = Cast.AsString(Data["hthomescore"]) + " - " +
                         Cast.AsString(Data["htawayscore"]);
        }

        public override void AssignFinalScoreLabel()
        {
            FinalScoreLabel = Resources.Lang.firsthalfscore;
        }
    }
}
