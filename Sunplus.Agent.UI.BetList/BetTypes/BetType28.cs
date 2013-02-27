namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// 3-way handicap
    /// </summary>
    public class BetType28 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);
            
            string teamName = Resources.BetList.lblDraw;
            string betteam = row["betteam"].ToString();
            decimal hdp1 = decimal.Parse(row["hdp1"].ToString());
            decimal hdp2 = decimal.Parse(row["hdp2"].ToString());

            string handicap = string.Empty;

            if ("1" == betteam)
            {
                teamName = Home;
                handicap = Formater.FormatSignNumber((int)(hdp2 - hdp1));
            }
            else if ("2" == betteam)
            {
                teamName = Away;
                handicap = Formater.FormatSignNumber((int)(hdp1 - hdp2));
            }
            else
            {
                this.Event.betTeamClassName = BL_FAVORITE;
                teamName = Resources.BetList.lblDraw;
                if (hdp1 - hdp2 > 0)
                {
                    handicap = Resources.BetList.a + "&nbsp;";
                }
                else if (hdp1 - hdp2 < 0)
                {
                    handicap = Resources.BetList.h + "&nbsp;";
                }
                else
                {
                    handicap = string.Empty;
                }

                handicap += Formater.FormatSignNumber((int)Math.Abs(hdp2 - hdp1));
            }

            this.Event.betTeam = string.Join(null, new string[] { teamName, "&nbsp;(", handicap.ToString(), ")" });
            
            this.Event.Handicap = null;            
        }
    }
}
