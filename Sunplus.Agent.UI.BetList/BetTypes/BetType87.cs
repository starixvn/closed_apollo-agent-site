namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// Next High/Low
    /// </summary>
    public class BetType87 : BetType1
    {        
        public override void AssignEvent(DataRow row)
        {         
            base.AssignEvent(row);
            
            string betId = row["betId"].ToString();
            if (betId == "0")
            {
                betId = string.Empty;
            }

            string liveScore = (!IsLive || betId == string.Empty) ? " [0]" : " [" + betId + "]";            
            
            string betTeam = row["betteam"].ToString() == "h" ? Resources.BetList.high : Resources.BetList.low;

            string bettypeName = this.Event.betTypeName;
            
            this.Event.betTeam = betTeam + liveScore;
            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.betTypeName = bettypeName;
            this.Event.Score = null;
            this.Event.VS = null;
            this.Event.homeTeam = Resources.BetList.numbergameno + " " + row["matchcode"];
            this.Event.Handicap = null;            
        }
    }
}
