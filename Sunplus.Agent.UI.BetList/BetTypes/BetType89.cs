namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// Next Combo
    /// do NOT show oddtype because this bettype just offer for DEC
    /// it supports for live ball, so always so 37.5
    /// </summary>
    public class BetType89 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);
            oddType = string.Empty;
        }

        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);

            string betId = row["betId"].ToString();
            if (betId == "0")
            {
                betId = string.Empty;
            }

            string liveScore = (!IsLive || betId == string.Empty) ? " [0]" : " [" + betId + "]";

            string betTeam = string.Empty;
            switch (row["betteam"].ToString())
            {
                case "1:1": betTeam = Resources.BetList.lblOverOdd;
                    break;
                case "1:2": betTeam = Resources.BetList.lblOverEven;
                    break;
                case "2:1": betTeam = Resources.BetList.lblUnderOdd;
                    break;
                case "2:2": betTeam = Resources.BetList.lblUnderEven;
                    break;
            }

            if (this.ExportExcel)
            {
                betTeam += " 37.5";
            }
            else
            {
                betTeam += " <span style='color:#555555'>37.5</span> ";
            }

            this.Event.betTeam = betTeam + liveScore;
            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.Score = null;
            this.Event.VS = null;
            this.Event.homeTeam = Resources.BetList.numbergameno + " " + row["matchcode"];
            this.Event.Handicap = null;
        }
    }
}