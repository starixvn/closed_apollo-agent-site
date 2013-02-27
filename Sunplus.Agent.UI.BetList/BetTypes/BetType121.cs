namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.UI.BetList.Utils;

    /// <summary>
    /// Home no bet
    /// </summary>
    public class BetType121 : BetType1
    {
        private string home = string.Empty;
        private string away = string.Empty;
        private bool isLive = false;

        public override void AssignEvent(System.Data.DataRow row)
        {
            System.Data.DataColumnCollection columns = row.Table.Columns;

            // Set values
            if (this.LeagueID == null)
            {
                this.LeagueID = this.GetValueFromDataRow(row, "leagueId", 0);
            }

            if (this.BetTypeID == null)
            {
                this.BetTypeID = this.GetValueFromDataRow(row, "betType", 0);
            }

            if (this.SportTypeID == null)
            {
                this.SportTypeID = this.GetValueFromDataRow(row, "sportType", 0);
            }

            string betTeam = this.GetValueFromDataRow(row, "betteam").ToString();
            bool betHomeTeam = betTeam == "x";
            decimal hdp1 = columns.Contains("hdp1") ? Convert.ToDecimal(row["hdp1"]) : 0;
            decimal hdp2 = columns.Contains("hdp2") ? Convert.ToDecimal(row["hdp2"]) : 0;

            bool isNeutral = columns.Contains("isneutral") ? (bool)row["isneutral"] : false;

            this.home = CacheRef.GetTeamName(this.GetValueFromDataRow(row, "homeid"));
            this.away = CacheRef.GetTeamName(this.GetValueFromDataRow(row, "awayid"));

            this.Event.betTeam = betHomeTeam ? Resources.Lang.lblDraw : this.away;
            this.Event.betTeamClassName = this.Event.Score.scoreClassName = betHomeTeam ? (hdp2 >= hdp1 ? BL_UNDER_DOG : BL_FAVORITE) : (hdp2 <= hdp1 ? BL_UNDER_DOG : BL_FAVORITE);
            this.Event.Handicap.handicap = null;

            this.Event.Score.homeScore = this.GetValueFromDataRow(row, "livehomescore").ToString();
            this.Event.Score.awayScore = this.GetValueFromDataRow(row, "liveawayscore").ToString();

            this.isLive = Convert.ToBoolean(row["liveindicator"]);

            // For special cases
            this.Event.Score.Visible = this.isLive;

            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID));

            this.Event.homeTeam = this.home + (isNeutral ? " (N)" : string.Empty);
            this.Event.home_firstGoal_lastGoal = string.Empty;
            this.Event.awayTeam = this.away;
            this.Event.away_firstGoal_lastGoal = string.Empty;
            this.Event.strikeThrough = string.Empty;

            if (columns.Contains("betstatus"))
            {
                this.CalculateFG_LG(row["betstatus"] as string, ref this.Event.home_firstGoal_lastGoal, ref this.Event.away_firstGoal_lastGoal);
            }

            this.Event.sportTypeName = CacheRef.GetSportName(Convert.ToInt32(this.SportTypeID));
            this.Event.LeagueName.leagueName = CacheRef.GetLeagueName(Convert.ToInt32(this.LeagueID));
        }
    }
}