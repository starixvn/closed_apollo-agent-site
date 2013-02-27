namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;

    /// <summary>
    /// Double Chance
    /// </summary>
    public class BetType24 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);

            string team = row["betteam"].ToString().Contains("1") ? Home : Away;
            string draw_away = row["betteam"].ToString().EndsWith("x") ? Resources.BetList.lblDraw : this.Event.awayTeam;

            this.Event.betTeam = string.Join(
                                            null, 
                                            new string[] 
                                            { 
                                                " <span class=\"",
                                                BL_FAVORITE,
                                                "\">",
                                                team,
                                                "</span>",
                                                "&nbsp;",
                                                Resources.BetList.lblOr.ToLower(),
                                                " <span class=\"",
                                                BL_FAVORITE,
                                                "\">",
                                                "&nbsp;",
                                                draw_away,
                                                "</span>" 
                                            });

            if (this.Event.Score.Visible)
            {
                this.Event.Score.scoreClassName = BL_FAVORITE;
            }            

            this.Event.Handicap = null;
        }
    }
}
