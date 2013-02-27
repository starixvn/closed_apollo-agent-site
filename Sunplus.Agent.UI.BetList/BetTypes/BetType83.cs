namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// 1st ball OE
    /// </summary>
    public class BetType83 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);
            this.Event.betTeam = row["betteam"].ToString() == "h" ? Resources.BetList.odd : Resources.BetList.even;
            this.Event.betTeamClassName = BL_FAVORITE;            
            this.Event.VS = null;
            this.Event.homeTeam = Resources.BetList.numbergameno + " " + row["matchcode"];
            this.Event.Handicap = null;
        }
    }
}
