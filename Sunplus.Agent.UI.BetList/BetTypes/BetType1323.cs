///  Set x Total Deuce Games 2-way inherit from Over/Under
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    ///  Set x Total Deuce Games 2-way
    /// </summary>
    public class BetType1323 : BetType3
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

            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
            this.Event.betTeamClassName = BL_FAVORITE;
        }
    }
}
