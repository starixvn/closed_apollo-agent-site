/// Over/Under inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using Sunplus.Agent.UI.BetList.Utils;

    /// <summary>
    /// Over/Under
    /// </summary>
    public class BetType3 : BetType1
    {
        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);

            // Bet team
            string betTeam = row["betteam"] as string;
            bool betOver = betTeam == "h";

            this.Event.betTeam = betOver ? Resources.BetList.over : Resources.BetList.under;
            if (this.Event.Score.Visible)
            {
                this.Event.betTeamClassName = this.Event.Score.scoreClassName = BL_FAVORITE;
            }

            this.Event.Handicap.handicap = ConvertByBetType.Hdp(Convert.ToDecimal(row["hdp1"]));
        }
    }
}
