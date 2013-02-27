namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// Next Over/Under
    /// it supports for live ball, so always so 37.5
    /// </summary>
    public class BetType85 : BetType3
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

            if (this.ExportExcel)
            {
                betTeam += " 37.5";
            }
            else
            {
                betTeam += " <span style='color:#555555'>37.5</span> ";
            }

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
            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.betTypeName = bettypeName;
            this.Event.Score = null;
            this.Event.VS = null;
            this.Event.homeTeam = Resources.BetList.numbergameno + " " + row["matchcode"];
            this.Event.Handicap = null;
        }
    }
}