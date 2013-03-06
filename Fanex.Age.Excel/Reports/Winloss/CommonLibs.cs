namespace Fanex.Age.Excel.Reports.Winloss
{
    using Sunplus.Agent.Model.Entities.Permission;

    internal class CommonLibs
    {
        public static string GetBetTypeName(int betType, int liveIndicator)
        {
            string betTypeName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetBettype(betType);
            string casinoType = string.Empty;
            switch (betType)
            {
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Casino_Games:  // 71
                    casinoType = "Casino";
                    break;

                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.P2P: // 1000
                    betTypeName = Resources.Lang.P2P;
                    casinoType = "P2P";
                    break;

                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Bingo: // 73

                    break;

                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Over_Under: // Number game Over/Under
                    betTypeName = Resources.Lang.next + " " + betTypeName + " (" + Resources.Lang.numbergame + ")";
                    break;

                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Odd_Even: // Number game Odd/Even
                    string strNextCombo = liveIndicator == 1 ? Resources.Lang.next + " " : Resources.Lang.ft + ". ";
                    betTypeName = strNextCombo + betTypeName + " (" + Resources.Lang.numbergame + ")";
                    break;

                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.NumberWheel: // Number wheel
                    string live = liveIndicator == 1 ? Resources.Lang.LiveGame : Resources.Lang.PreGame;
                    betTypeName = betTypeName + " (" + live + ")";
                    break;
                //// For Live Casino
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Baccarat: // 1101
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Roulette: // 1102
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Sicbo: // 1103
                    if (UserPermission.IsShowLiveCasino)
                    {
                    }

                    break;
                // For Virtual Sports
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VS_1X2:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VS_CorrectScore:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VS_DoubleChange:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VS_Handicap:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VS_OverUnder:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VS_TotalGoal:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VHR_Win:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VHR_Place:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VHR_WinPlace:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VHR_Forecast:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VHR_Tricast:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VT_PlayerWin:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VT_ScoreBet:
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.VT_TotalPoints:
                    betTypeName = betTypeName + " (" + Resources.Lang.VirtualSports + ")";
                    break;
                default:
                    break;
            }

            return betTypeName;
        }
    }
}