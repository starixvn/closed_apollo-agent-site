
namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.Cache;
    /// <summary>
    /// Home no bet
    /// </summary>

    public class BetType121 : BetType1
    {
        public override void AssignHdp()
        {
            Hdp = "--";
        }

        public override void AssignChoice()
        {
            var betTeam = Data["betTeam"].ToString();
            var away = CacheRef.GetTeamName(Data["awayid"]);
            Choice = betTeam.Contains("x") ? Resources.Lang.lblDraw : away;
        }
    }
}