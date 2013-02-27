namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// Both/One/Neither team to score
    /// </summary>
    public class BetType26 : BetType24
    {
        public override void AssignChoice()
        {
            var betTeam = Data["betteam"].ToString();
            switch (betTeam)
            {
                case "b":
                    Choice = Resources.BetList.lblBoth;
                    break;
                case "o":
                    Choice = Resources.BetList.lblOne;
                    break;
                case "n":
                    Choice = Resources.BetList.nogoal;
                    break;
            }
        }
    }
}
