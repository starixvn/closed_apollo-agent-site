namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// Next Odd/Even
    /// </summary>
    public class BetType86 : BetType2
    {        
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);
                    
            string betId = row["betId"].ToString();
            if (betId == "0")
            {
                betId = string.Empty;
            }

            string liveScore = string.Empty;

            string betTeam = this.Event.betTeam;

            string bettypeName = this.Event.betTypeName;
            if (!IsLive)
            {
                bettypeName = Resources.BetList.ft + ". " + bettypeName;                
            }
            else
            {
                bettypeName = Resources.BetList.next + " " + bettypeName;
                liveScore = (!IsLive || betId == string.Empty) ? " [0]" : " [" + betId + "]";
            }

            this.Event.betTeam = betTeam + liveScore;            
            this.Event.betTypeName = bettypeName;
            this.Event.Score = null;
            this.Event.VS = null;
            this.Event.homeTeam = Resources.BetList.numbergameno + " " + row["matchcode"];            
        }
    }
}
