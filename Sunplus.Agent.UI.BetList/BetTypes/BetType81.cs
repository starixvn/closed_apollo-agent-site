namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// 1st ball OU
    /// </summary>
    public class BetType81 : BetType1
    {       
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);

            this.Event.betTeam = row["betteam"].ToString() == "h" ? Resources.BetList.over : Resources.BetList.under;
            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.Handicap.handicap = "37.5";
            this.Event.VS = null;
            this.Event.homeTeam = Resources.BetList.numbergameno + " " + row["matchcode"];         
        }
    }
}
