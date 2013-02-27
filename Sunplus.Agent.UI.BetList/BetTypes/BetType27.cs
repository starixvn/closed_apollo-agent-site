namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// To win to nil
    /// </summary>
    public class BetType27 : BetType1
    {
        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);
                         
            this.Event.Handicap = null;            
        }
    }
}
