namespace Sunplus.Agent.UI.BetSlip.UI
{
    /// <summary>
    /// Warrior
    /// </summary>
    public class BetType88 : BetType87
    {
        public override void AssignChoice()
        {
            var betTeam = Data["betteam"];
            Choice = betTeam.Equals("h") ? Resources.BetList.warrior2ndBall : Resources.BetList.warrior3rdBall;
        }
    }
}
