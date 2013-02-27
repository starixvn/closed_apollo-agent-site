
using Sunplus.Agent.Utilities;

namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// Odd/Even
    /// </summary>
    public class BetType2 : BetType1
    {
        public override void AssignChoice()
        {
            var betTeam = Cast.AsString(Data["betteam"]);
            Choice = betTeam == "h" ? Resources.BetList.odd.ToUpper() : Resources.BetList.even;
        }

        public override void AssignHdp()
        {
            Hdp = "--";
        }
    }
}
