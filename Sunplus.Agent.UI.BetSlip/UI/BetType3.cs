
using Sunplus.Agent.Utilities;

namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// Over/Under
    /// </summary>
    public class BetType3 : BetType1
    {
        public override void AssignChoice()
        {
            var betTeam = Cast.AsString(Data["betteam"]);
            Choice = betTeam == "h" ? Resources.BetList.over : Resources.BetList.under;
        }

        public override void AssignHdp()
        {
            var hdp1 = Cast.AsDecimal(Data["hdp1"]);
            Hdp = Formater.DecFormat(hdp1, 2);
        }
    }
}
