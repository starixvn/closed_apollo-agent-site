namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;

    /// <summary>
    /// 1st 1X2
    /// </summary>
    public class BetType15 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);

            this.Event.betTeam = string.Join(null, new string[] { Resources.BetList.ht, ".", row["betteam"] as string });
            this.Event.betTeamClassName = BL_FAVORITE;
            this.Event.Handicap = null;
        }
    }
}
