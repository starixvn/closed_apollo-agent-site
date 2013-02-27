///  Set x Total Games 3-way inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.UI.BetList.Utils;

    /// <summary>
    ///    Set x Total Games 3-way
    /// </summary>
    public class BetType1313 : BetType1
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
            bool betOver = betTeam == "1";
            bool betUnder = betTeam == "2";
            decimal line1;
            decimal line2;
            decimal.TryParse(row["hdp1"].ToString(), out line1);
            decimal.TryParse(row["hdp2"].ToString(), out line2);

            this.Event.betTeam = betOver ? Resources.BetList.over : betUnder ? Resources.BetList.under : string.Format(Resources.BetList.betweenXandY, ConvertByBetType.Hdp(line2), ConvertByBetType.Hdp(line1));
            this.Event.Handicap.handicap = betOver ? ConvertByBetType.Hdp(line1) : betUnder ? ConvertByBetType.Hdp(line2) : null;
            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
            this.Event.betTeamClassName = BL_UNDER_DOG;
        }
    }
}
