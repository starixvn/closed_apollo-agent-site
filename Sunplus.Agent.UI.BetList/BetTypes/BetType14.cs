namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.UI.BetList.Utils;

    /// <summary>
    /// First Goal/Last Goal
    /// </summary>
    public class BetType14 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);

            this.Event.betTeam = ConvertByBetType.GetBetTeam(row["betteam"].ToString(), Convert.ToInt32(BetTypeID));
            this.Event.betTeamClassName = BL_FAVORITE;
            this.Event.Handicap = null;
        }
    }
}