///    Set x Games y & y+1 Points inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.UI.BetList.Utils;

    /// <summary>
    ///      Set x Games y & y+1 Points
    /// </summary>
    public class BetType1334 : BetType1
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
            bool team1 = betTeam == "1";
            bool team2 = betTeam == "2";

            this.Event.betTeam = team1 ? Resources.BetList.under : team2 ? Resources.BetList.over : Resources.BetList.exactly;
            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
            this.Event.Handicap.handicap = ConvertByBetType.Hdp(Convert.ToDecimal(row["hdp1"]));
        }
    }
}
