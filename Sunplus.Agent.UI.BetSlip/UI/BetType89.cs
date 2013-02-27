namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// Next Combo
    /// </summary>
    public class BetType89 : BetType87
    {
        public override void AssignChoice()
        {
            var betTeam = Data["betteam"].ToString();
            switch (betTeam)
            {
                case "1:1":
                    Choice = Resources.BetList.lblOverOdd;
                    break;
                case "1:2":
                    Choice = Resources.BetList.lblOverEven;
                    break;
                case "2:2":
                    Choice = Resources.BetList.lblUnderEven;
                    break;
                case "2:1":
                    Choice = Resources.BetList.lblUnderOdd;
                    break;
            }
        }
    }
}
