namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// Draw no bet
    /// </summary>
    public class BetType25 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);
            this.Event.Handicap = null;
        }
    }
}
