namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    /// Match Total Games Odd/Even
    /// </summary>
    public class BetType1305 : BetType2
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
