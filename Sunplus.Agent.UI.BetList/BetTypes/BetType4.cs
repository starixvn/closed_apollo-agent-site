/// Correct score inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    /// <summary>
    /// Correct score
    /// </summary>
    public class BetType4 : BetType1
    {
        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);

            string choice = row["betteam"] as string;
            choice = (choice == "0:5" || choice == "5:0") ? choice + " UP" : choice;

            // "&nbsp" because export format
            this.Event.betTeam = "&nbsp;" + choice;
            this.Event.betTeamClassName = BL_FAVORITE;
            this.Event.Handicap = null;
        }
    }
}
