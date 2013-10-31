/*
 * Created 20091224@Lee - Common definitions
 * Revision:
 * 20091224@Lee - Initializing
 * 
 * !!!CAUTION: THIS IS GLOBAL DEFINITIONS, ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * Please refer to <document url> for more details
 * 
 * 
 */

namespace Fanex.Age.UI
{
    /// <summary>
    /// Common class.
    /// </summary>
    public class Common
    {
        #region Background color setting

        // Note that we use bgColor, don't use class Name because performance problem
        public const string RowBgOdd = "#F6F8F9";
        public const string RowBgEven = "#faf9ee";
        public const string RowBgOver = "#FBF0B9";
        public const string RowBgClosed = "#ccc7ba";
        public const string RowBgSuspended = "#f8ded7";
        public const string RowBgLive = "#ffddcc";
        public const string RowBgDisabled = "#A0A97A";

        public const string RowCssOdd = "BgOdd";
        public const string RowCssEven = "BgEven";
        public const string RowCssOver = "BgOver";
        public const string RowCssClosed = "BgClo";
        public const string RowCssSuspended = "BgSus";
        public const string RowCssLive = "BgLive";
        public const string RowCssDisabled = "BgDis";

        #endregion

        // Checked status for checkbox.
        public const string CHECKED = " checked";
        public const string DISABLED = " disabled";

        #region BetList

        /// <summary>
        /// Best List Win loss by date.
        /// </summary>
        public const string BetListWinLossByDate = "WLByDate";

        /// <summary>
        /// BestList Win loss by date old.
        /// </summary>
        public const string BetListWinLossByDateOld = "WLByDate_Old";

        /// <summary>
        /// Win Loss By bet type Match.
        /// </summary>
        public const string WinLossByBetTypeMatch = "WLByBettypeMatch";

        /// <summary>
        /// Bet List Bettype Match (Hdp_OU_Live_TG_OE_Outright_ML).
        /// </summary>
        public const string BetListByBetTypeMatch = "BettypeMatch";

        /// <summary>
        /// BetList Bettype Match (RACING).
        /// </summary>
        public const string RacingBetListByBetTypeMatch = "RacingBettypeMatch";

        /// <summary>
        /// BetList Running Bet type Match.
        /// </summary>
        public const string RunningBetListByBetTypeMatch = "RunningBettypeMatch";

        /// <summary>
        /// Best List by Bet type
        /// 1X2_CS_MP.
        /// </summary>
        public const string BetListByBetType = "Bettype";

        /// <summary>
        /// Best List by Mix Parlay.
        /// </summary>
        public const string BestListByMixParlay = "MixParlay";

        /// <summary>
        /// Best List by Bet type only Mix parlay of Match.
        /// </summary>
        public const string BetListByMixParlayOfMatch = "MixParlayByMatch";

        /// <summary>
        /// Bet list running by date on member.
        /// Only for agent - (Member Outstanding).
        /// </summary>
        public const string BetListRunningByDate_Member = "RunByDate_Mem";

        /// <summary>
        /// Bet list running by bet type match on member.
        /// Only for agent - (Member Outstanding).
        /// </summary>
        public const string BetListRunningByBetTypeMatch_Member = "RunByBtypeM";

        /// <summary>
        /// Bet list by all date.
        /// Only for agent - (Member Outstanding).
        /// </summary>
        public const string BetListByAllDate = "AllDate";

        /// <summary>
        /// Best List Select Deleted.
        /// </summary>
        public const string BetListByDeleted = "Deleted";

        /// <summary>
        /// Best List By Daily Statement.
        /// </summary>
        public const string BetListByDailyStatement = "BLDailyST";

        #endregion
    }
}