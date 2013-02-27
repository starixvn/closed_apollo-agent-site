///    Set x Game y Deuce Betting inherit from Money Line
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    ///    Set x Game y Deuce Betting
    /// </summary>
    public class BetType1327 : BetType20
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
            bool yes = betTeam == "h";

            this.Event.betTeam = yes ? Resources.Lang.lblYes : Resources.Lang.lblNo;
            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
        }
    }
}
