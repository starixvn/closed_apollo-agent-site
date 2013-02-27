
namespace Sunplus.Agent.UI.BetSlip.UI
{
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    /// Away no bet
    /// </summary>
    public class BetType122 : BetType1
    {
        public override void AssignHdp()
        {
            Hdp = "--";
        }

        public override void AssignChoice()
        {
            var betTeam = Data["betTeam"].ToString();
            var home = CacheRef.GetTeamName(Data["homeid"]);
            Choice = betTeam.Contains("h") ? home : Resources.Lang.lblDraw;
        }
    }
}