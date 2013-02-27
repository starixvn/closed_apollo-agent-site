namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.Cache;
    /// <summary>
    /// Draw no draw
    /// </summary>

    public class BetType123 : BetType1
    {
        public override void AssignHdp()
        {
            Hdp = "--";
        }

        public override void AssignChoice()
        {
            var betTeam = Data["betTeam"].ToString();
            Choice = betTeam.Contains("h") ? Resources.Lang.lblDraw : Resources.Lang.lblNoDraw;
        }
    }
}