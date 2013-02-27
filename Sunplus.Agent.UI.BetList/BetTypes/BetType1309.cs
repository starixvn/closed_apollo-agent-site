///  Match Handicap Games 3-way inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.UI.BetList.Utils;

    /// <summary>
    ///    Match Handicap Games 3-way
    /// </summary>
    public class BetType1309 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            // modify            
            TransTime.Show();
            TransTimeHyperLink.Hide();
        }

        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);

            System.Data.DataColumnCollection columns = row.Table.Columns;
            string betTeam = row["betteam"] as string;
            bool betHome = betTeam == "1";
            bool betAway = betTeam == "2";
            decimal hdp1 = columns.Contains("hdp1") ? Convert.ToDecimal(row["hdp1"]) : 0;
            decimal hdp2 = columns.Contains("hdp2") ? Convert.ToDecimal(row["hdp2"]) : 0;
            decimal handicap = hdp2 > hdp1 ? hdp2 : hdp1;

            if (betHome)
            {
                this.Event.betTeam = this.Home;
                this.Event.betTeamClassName = this.Event.Score.scoreClassName = (hdp2 >= hdp1 ? BL_UNDER_DOG : BL_FAVORITE);
                this.Event.Handicap.handicap = ConvertByBetType.Hdp((this.Event.betTeamClassName == BL_FAVORITE) ? -handicap : handicap);
            }
            else if (betAway)
            {
                this.Event.betTeam = this.Away;
                this.Event.betTeamClassName = this.Event.Score.scoreClassName = (hdp2 <= hdp1 ? BL_UNDER_DOG : BL_FAVORITE);
                this.Event.Handicap.handicap = ConvertByBetType.Hdp((this.Event.betTeamClassName == BL_FAVORITE) ? -handicap : handicap);
            }
            else
            {
                this.Event.betTeamClassName = this.Event.Score.scoreClassName = BL_UNDER_DOG;
                this.Event.betTeam = hdp2 > hdp1 ? Resources.Lang.lblDraw + "&nbsp;(" + Resources.BetList.h + "+" + ConvertByBetType.Hdp(hdp2) + ")" :
                                                    Resources.Lang.lblDraw + "&nbsp;(" + Resources.BetList.a + "+" + ConvertByBetType.Hdp(hdp1) + ")";
                this.Event.Handicap.handicap = null;
            }

            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
        }
    }
}
