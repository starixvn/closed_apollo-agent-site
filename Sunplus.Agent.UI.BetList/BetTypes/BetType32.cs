namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    /// Place
    /// </summary>
    public class BetType32 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            // modify            
            TransTime.Show();
            TransTimeHyperLink.Hide();
        }

        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);
            this.Event.Handicap = null;

            this.Event.betTeam = row["matchcode"] + " - " + CacheRef.GetHorseTeamName(row["homeid"]);
            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.homeTeam = Resources.BetList.race + " " + row["race"];
            this.Event.VS = null;

            this.Event.home_firstGoal_lastGoal = null;
            this.Event.away_firstGoal_lastGoal = null;
            this.Event.awayTeam = null;

            this.Status.StatusResult.race = GetValueFromDataRow(row, "race", 0).ToString();
        }
    }
}
