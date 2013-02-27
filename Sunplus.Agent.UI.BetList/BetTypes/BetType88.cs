namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// Warrior    
    /// </summary>
    public class BetType88 : BetType1
    {        
        public override void AssignEvent(DataRow row)
        {         
            base.AssignEvent(row);
            
            string betTeam = row["betteam"].ToString() == "h" ? Resources.BetList.warrior2ndBall : Resources.BetList.warrior3rdBall;

            string bettypeName = this.Event.betTypeName;

            this.Event.betTeam = betTeam;
            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.betTypeName = bettypeName;
            this.Event.Score = null;
            this.Event.VS = null;
            this.Event.homeTeam = Resources.BetList.numbergameno + " " + row["matchcode"];
            this.Event.Handicap = null;            
        }
    }
}
