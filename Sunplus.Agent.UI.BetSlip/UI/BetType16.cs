/*
 * Created 20111114@Kaden - Refactor BetSlip
 * Revision:
 * 20111114@Kaden: Get BetTeam
 * 
 */

using Sunplus.Agent.Utilities;
using Sunplus.Agent.UI.BetSlip.Utils;

namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// HT/FT
    /// </summary>
    public class BetType16 : BetType5
    {
        public override void AssignChoice()
        {
            Choice = ConvertOddsByBetTypes.GetBetTeam(Data["betteam"].ToString(), Cast.AsInt32(Data["bettype"]));
        }
    }
}
