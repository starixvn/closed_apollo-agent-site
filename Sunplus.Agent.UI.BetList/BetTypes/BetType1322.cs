///    Set x Score after 6 Games inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    ///      Set x Score after 6 Games
    /// </summary>
    public class BetType1322 : BetType1
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
            if (choice.Length == 2)
            {
                betTeam = choice.Substring(0, 1) + ":" + choice.Substring(1, 1);
            }

            // "&nbsp" because export format
            this.Event.betTeam = "&nbsp;" + betTeam;
            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
            this.Event.Handicap.handicap = null;
        }
    }
}
