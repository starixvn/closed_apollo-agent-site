namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Configuration;
    using System.Data;

    /// <summary>
    /// Both/One/Neither team to score
    /// </summary>
    public class BetType26 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);
            
            string betteam = row["betteam"].ToString();

            if (betteam == "b")
            {
                this.Event.betTeam = Resources.BetList.lblBoth;
            }
            else if (betteam == "o")
            {
                this.Event.betTeam = Resources.BetList.lblOne;
            }
            else if (betteam == "n")
            {
                this.Event.betTeam = Resources.BetList.nogoal;
            }

            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.Handicap = null;            
        }
    }
}
