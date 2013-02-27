using Sunplus.Agent.Model.BL.Cache;

namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// Double Chance
    /// </summary>
    public class BetType24 : BetType1
    {
        public override void AssignHdp()
        {
            Hdp = "--";
        }

        public override void AssignChoice()
        {
            var betTeam = Data["betTeam"].ToString();
            var home = CacheRef.GetTeamName(Data["homeid"]); 
            var away = CacheRef.GetTeamName(Data["awayid"]); 
            const string choiceTemplate = "{0} {1} {2}";
            var chance1 = betTeam.Contains("1") ? home : away;
            var chance2 = betTeam.EndsWith("x") ? Resources.BetList.lblDraw : away;
            Choice = string.Format(choiceTemplate, chance1, Resources.BetList.lblOr.ToLower(), chance2);            
        }
    }
}
