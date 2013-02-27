
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Definitions;


namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// 1st ball OU
    /// </summary>
    public class BetType81 : BetType1
    {
        public override void AssignVs()
        {
            var matchCode = Cast.AsString(Data["matchcode"]);
            VS = Resources.BetList.numbergameno + "&nbsp;" + matchCode;            
        }

        public override void AssignChoice()
        {
            var betTeam = Data["betteam"].ToString();
            Choice = betTeam.Equals("h") ? Resources.BetList.over : Resources.BetList.under;
        }

        public override void AssignHdp()
        {
            Hdp = "--";
        }

        public override void AssignLiveScore()
        {
            LiveScore = Cast.AsString(Data["betid"]);
        }

        public override void AssignFinalScore()
        {
            const string temmplate = "<div onclick=\"ViewResult('{0}', '{1}', 0 , '{2}');\" class=\"link\">{3}</div>";


            FinalScore = string.Format(temmplate, Data["matchid"], (int)SportType.NumberGame, Data["bettype"],
                Resources.BetList.results);
        }
    }
}
