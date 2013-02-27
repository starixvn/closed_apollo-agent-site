///    Set x Game y Total Points Exact inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    ///      Set x Game y Total Points Exact
    /// </summary>
    public class BetType1328 : BetType1
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

            string choice = row["betteam"] as string;
            var betTeam = string.Empty;
            switch (choice)
            {
                case "01":
                    betTeam = "4 " + Resources.BetList.points;
                    break;
                case "02":
                    betTeam = "5 " + Resources.BetList.points;
                    break;
                case "03":
                    betTeam = "6 " + Resources.BetList.points;
                    break;
                case "04":
                    betTeam = "7 " + Resources.BetList.upPoints;
                    break;
                default:
                    break;
            }

            // "&nbsp" because export format
            this.Event.betTeam = "&nbsp;" + betTeam;
            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
            this.Event.Handicap.handicap = null;
        }
    }
}
