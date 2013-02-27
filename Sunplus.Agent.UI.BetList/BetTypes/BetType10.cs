namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    /// Outright
    /// </summary>
    public class BetType10 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            // modify            
            TransTime.Show();
            TransTimeHyperLink.Hide();
            Status.StatusResult.Visible = false;
            Status.StatusResult.is_outright = "1";
        }

        public override void AssignEvent(System.Data.DataRow row)
        {            
            // For special cases
            if (!IsLive) 
            {
                this.Event.Score = null;
            }

            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(BetTypeID));

            string home = CacheRef.GetTeamName(row["homeid"]);
            this.Event.betTeam = home;
            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.Handicap = null;
            this.Event.homeTeam = null;
            this.Event.awayTeam = null;
            this.Event.VS = null;
            this.Event.home_firstGoal_lastGoal = null;
            this.Event.away_firstGoal_lastGoal = null;

            this.Event.sportTypeName = CacheRef.GetSportName(Convert.ToInt32(SportTypeID));
            this.Event.LeagueName.leagueName = CacheRef.GetLeagueName(Convert.ToInt32(LeagueID));                          
        }
    }
}
