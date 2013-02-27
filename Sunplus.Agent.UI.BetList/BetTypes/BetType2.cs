/// Odd/Even inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    /// <summary>
    /// Odd/Even
    /// </summary>
    public class BetType2 : BetType1
    {
        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);

            string betTeam = row["betteam"] as string;
            bool betOdd = betTeam == "h";

            this.Event.Handicap = null;
            this.Event.betTeam = betOdd ? Resources.BetList.odd : Resources.BetList.even;
            this.Event.betTeamClassName = BL_FAVORITE;
        }
    }
}
