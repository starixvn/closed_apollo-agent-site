///  Set x Player to win on a Tiebreak inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    ///    Set x Player to win on a Tiebreak
    /// </summary>
    public class BetType1315 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            // modify            
            string betId = row["betid"].ToString();
            string refNo = "0";
            if (betId.Length > 2)
            {
                refNo = betId.Substring(1, 2);
            }

            TransTime.Show();
            TransTimeHyperLink.Hide();
            this.Status.StatusResult.refNo = refNo;
        }

        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);
            string betTeam = row["betteam"] as string;
            bool home = betTeam == "1";
            bool away = betTeam == "2";

            this.Event.betTeam = home ? this.Home : away ? this.Away : Resources.BetList.noTiebreak;
            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
            this.Event.Handicap.handicap = null;
        }
    }
}
