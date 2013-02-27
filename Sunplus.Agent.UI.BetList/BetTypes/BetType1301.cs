///  Match Winner inherit from Money Line
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    ///  Match Winner
    /// </summary>
    public class BetType1301 : BetType20
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

            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
            
        }
    }
}
