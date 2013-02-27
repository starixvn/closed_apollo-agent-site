namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;

    /// <summary>
    /// Clean Sheet
    /// </summary>
    public class BetType13 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);

            string team = row["betteam"].ToString().Contains("h") ? Home : Away;
            string yesNo = row["betteam"].ToString().Contains("y") ? Resources.BetList.lblYes : Resources.BetList.lblNo;
            this.Event.betTeam = string.Join(
                null, 
                new string[] 
                        {
                            team,
                            " <span style=\"text-transform: uppercase;\" class=\"", BL_FAVORITE, "\">",
                            yesNo,
                            "</span>" 
                        });
                                      
            this.Event.Handicap = null;
        }
    }
}
