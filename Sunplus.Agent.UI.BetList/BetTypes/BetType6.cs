namespace Sunplus.Agent.UI.BetList.BetTypes
{
    /// <summary>
    /// Total goal
    /// </summary>
    public class BetType6 : BetType1
    {
        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);

            // because export format
            this.Event.betTeam = "&nbsp;" + row["betteam"] as string;
            this.Event.betTeamClassName = BL_FAVORITE;
            this.Event.Handicap = null;
        }
    }
}
