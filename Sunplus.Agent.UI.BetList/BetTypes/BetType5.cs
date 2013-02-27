/// FT. 1x2 inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;

    /// <summary>
    /// FT. 1x2
    /// </summary>
    public class BetType5 : BetType1
    {
        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);

            this.Event.betTeamClassName = BL_FAVORITE;
            this.Event.Handicap = null;

            this.Event.betTeam = string.Join(null, new string[] { Resources.BetList.ft, ".", row["betteam"] as string });
        }
    }
}
