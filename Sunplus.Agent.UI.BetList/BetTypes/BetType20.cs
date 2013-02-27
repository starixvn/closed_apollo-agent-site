namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// Money Line
    /// </summary>
    public class BetType20 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);
            this.Event.Handicap = null;
        }
    }
}
