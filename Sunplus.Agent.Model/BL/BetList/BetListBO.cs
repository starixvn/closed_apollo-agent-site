/*
 * Created 20100507@Jacob - Business class of Bet List function
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.Model.BL.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data.DNAide;
    using Reports;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.DL.BetList;

    public class BetListBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public BetListBO(string netSessionId)
            : base(netSessionId)
        {
        }

        #region MixParlay

        /// <summary>
        /// Get Mix Parlay
        /// </summary>
        /// <param name="transid"></param>
        /// <returns></returns>
        public static DataTable GetMixParlayDetail(long transid, string language)
        {
            BetListDAO betListDAO = new BetListDAO();

            DataTable mixParlay = new DataTable();
            mixParlay = betListDAO.Age_BetList_SelParlayDetail(transid, language);

            return mixParlay;
        }

        /// <summary>
        /// Get Mix Parlay
        /// </summary>
        /// <param name="transid"></param>
        /// <returns></returns>
        public DataTable GetMixParlayDetail(long transid)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;

            DataTable mixParlay = new DataTable();
            mixParlay = betListDAO.Age_BetList_SelParlayDetail(transid, language);
            return mixParlay;
        }

        #endregion MixParlay

        #region WinLossOutright

        /// <summary>
        /// Get winloss outright
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="moveBetDate"></param>
        /// <returns></returns>
        public DataTable GetWinlossOutright(DateTime dateFrom, DateTime dateTo, DateTime moveBetDate)
        {
            BetListDAO betListDAO = new BetListDAO();
            int customerId = CurrentSession.CustId;
            string language = CurrentSession.LangKey;
            DataTable outright = null;
            if (CurrentSession.IsAgent)
            {
                outright = betListDAO.Age_BetList_SelWinlossOutright_Ag(customerId, dateFrom, dateTo, language, moveBetDate);
            }
            else if (CurrentSession.IsMaster)
            {
                outright = betListDAO.Age_BetList_SelWinlossOutright_MAg(customerId, dateFrom, dateTo, language, moveBetDate);
            }
            else if (CurrentSession.IsSuper)
            {
                outright = betListDAO.Age_BetList_SelWinlossOutright_SAg(customerId, dateFrom, dateTo, language, moveBetDate);
            }
            else
            {
                outright = new DataTable();
            }

            return outright;
        }

        #endregion WinLossOutright

        #region WinLossHR

        /// <summary>
        /// Get winloss horse racing
        /// </summary>
        /// <param name="betType"></param>
        /// <param name="leagueId"></param>
        /// <param name="race"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable GetWinlossHR(int betType, int leagueId, string race, DateTime dateFrom, DateTime dateTo, DateTime moveBetsDate, out DataTable outDataTable)
        {
            BetListDAO betListDAO = new BetListDAO();
            int customerId = CurrentSession.CustId;
            string language = CurrentSession.LangKey;
            DataTable horseRacing = null;
            if (CurrentSession.IsAgent)
            {
                horseRacing = betListDAO.Age_BetList_SelWinlossHR_Ag(customerId, betType, leagueId, race, dateFrom, dateTo, language, moveBetsDate, out outDataTable);
            }
            else if (CurrentSession.IsMaster)
            {
                horseRacing = betListDAO.Age_BetList_SelWinlossHR_MAg(customerId, betType, leagueId, race, dateFrom, dateTo, language, moveBetsDate, out outDataTable);
            }
            else if (CurrentSession.IsSuper)
            {
                horseRacing = betListDAO.Age_BetList_SelWinlossHR_SAg(customerId, betType, leagueId, race, dateFrom, dateTo, language, moveBetsDate, out outDataTable);
            }
            else
            {
                horseRacing = new DataTable();
                outDataTable = new DataTable();
            }

            return horseRacing;
        }

        #endregion WinLossHR

        #region WinlossByBettypeMatch

        /// <summary>
        /// Get winloss by bettype and match
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetWinlossByBettypeMatch(int matchId, int betType, string userName, int liveindicator, out DataTable outDataTable)
        {
            BetListDAO betListDAO = new BetListDAO();
            int customerId = CurrentSession.CustId;
            string language = CurrentSession.LangKey;
            DataTable bettypeMatch = null;
            if (CurrentSession.IsAgent)
            {
                bettypeMatch = betListDAO.Age_BetList_SelWinlossByBettypeMatch_Ag(customerId, matchId, betType, language, userName, liveindicator, out outDataTable);
            }
            else if (CurrentSession.IsMaster)
            {
                bettypeMatch = betListDAO.Age_BetList_SelWinlossByBettypeMatch_MAg(customerId, matchId, betType, language, userName, liveindicator, out outDataTable);
            }
            else if (CurrentSession.IsSuper)
            {
                bettypeMatch = betListDAO.Age_BetList_SelWinlossByBettypeMatch_SAg(customerId, matchId, betType, language, userName, liveindicator, out outDataTable);
            }
            else
            {
                bettypeMatch = new DataTable();
                outDataTable = new DataTable();
            }

            return bettypeMatch;
        }

        /// <summary>
        /// Get winloss by bettype and match
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetWinlossByBettypeMatch(int matchId, int betType, string userName, out DataTable dtbWinloss)
        {
            return this.GetWinlossByBettypeMatch(matchId, betType, userName, 0, out dtbWinloss);
        }

        #endregion WinlossByBettypeMatch

        #region RunningByDateAndMem

        /// <summary>
        /// Get running bets by date and member
        /// For only agent (Member Outstanding betlist)
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="getAll"></param>
        /// <param name="filterParams"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetRunningByDateAndMem(int custId, string userName, DateTime dateFrom, DateTime dateTo, int getAll, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable dataTable)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            DataTable runningBets = null;
            runningBets = betListDAO.Age_BetList_SelRunningByDate_Mem(custId, userName, dateFrom, dateTo, language, getAll, filterParams, out dataTable);
            return runningBets;
        }

        #endregion RunningByDateAndMem

        #region AllBetsByDateAndMem

        /// <summary>
        /// Get all bets by date and member
        /// For only agent (Member Outstanding betlist)
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="filterParams"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetAllBetsByDateAndMem(int custId, string userName, DateTime dateFrom, DateTime dateTo, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable outDataTable)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            DataTable allBets = null;
            int agentId = CurrentSession.CustId;
            allBets = betListDAO.Age_BetList_SelAllByDate_Mem(custId, agentId, userName, dateFrom, dateTo, language, filterParams, out outDataTable);
            return allBets;
        }

        #endregion AllBetsByDateAndMem

        #region GetDeletedBets

        /// <summary>
        /// Get deleted bets
        /// If user is agent or master, set roleid = 1
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public DataTable GetDeletedBets(int custId)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            DataTable deletedBets = null;
            if (CurrentSession.IsSuper)
            {
                deletedBets = betListDAO.Age_BetList_SelDeleted(custId, 3, language);
            }
            else
            {
                deletedBets = betListDAO.Age_BetList_SelDeleted(custId, 1, language);
            }

            return deletedBets;
        }

        /// <summary>
        /// Get deleted bets
        /// If user is agent or master, set roleid = 1
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetDeletedBets(int custId, out DataTable outDataTable)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            DataTable deletedBets = null;
            if (CurrentSession.IsSuper)
            {
                deletedBets = betListDAO.Age_BetList_SelDeleted(custId, 3, language, out outDataTable);
            }
            else
            {
                deletedBets = betListDAO.Age_BetList_SelDeleted(custId, 1, language, out outDataTable);
            }

            return deletedBets;
        }

        #endregion GetDeletedBets

        #region All Racing By Date

        /// <summary>
        /// Get all hr bets by match id
        /// </summary>
        /// <param name="betType"></param>
        /// <param name="matchId"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetAllRacingByMatch(int betType, int matchId, out DataTable outDataTable)
        {
            //BetListDAO betListDAO = new BetListDAO();

            //string language = CurrentSession.LangKey;
            //int customerId = CurrentSession.CustId;

            //DataTable racingBets = null;

            //bool enableRacing = Entities.Permission.UserPermission.IsShowRacing;

            //if (enableRacing && CurrentSession.IsAgent)
            //{
            //    racingBets = betListDAO.Age_BetList_SelAllByDateHR_Ag(customerId, betType, matchId, language, out outDataTable);
            //}
            //else if (enableRacing && CurrentSession.IsMaster)
            //{
            //    racingBets = betListDAO.Age_BetList_SelAllByDateHR_MAg(customerId, betType, matchId, language, out outDataTable);
            //}
            //else if (enableRacing && CurrentSession.IsSuper)
            //{
            //    racingBets = betListDAO.Age_BetList_SelAllByDateHR_SAg(customerId, betType, matchId, language, out outDataTable);
            //}
            //else
            //{
            //    racingBets = new DataTable();
            //    outDataTable = new DataTable();
            //}

            //return racingBets;

            throw new NotImplementedException();
        }

        #endregion All Racing By Date

        #region RunningByBettypeAndMatch

        /// <summary>
        /// Get Running bets by bettype and match for agent
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable GetRunningByBettypeAndMatch(int matchId, int betType, string betteam)
        {
            return GetRunningByBettypeAndMatch(matchId, betType, 0, betteam);
        }

        /// <summary>
        /// Get Running bets by bettype and match for agent
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="betteam"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetRunningByBettypeAndMatch(int matchId, int betType, string betteam, out DataTable outDataTable)
        {
            return GetRunningByBettypeAndMatch(matchId, betType, 0, betteam, out outDataTable);
        }

        /// <summary>
        /// Get Running bets by bettype and match for agent
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable GetRunningByBettypeAndMatch(int matchId, int betType, int liveindicator, string betteam)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            int customerId = CurrentSession.CustId;

            DataTable runningBets = new DataTable();
            //if (CurrentSession.IsAgent)
            //{
            //    runningBets = betListDAO.Age_BetList_SelRunningByBettypeMatch_Ag(matchId, customerId, betType, language, liveindicator, betteam);
            //}
            //else if (CurrentSession.IsMaster)
            //{
            //    runningBets = betListDAO.Age_BetList_SelRunningByBettypeMatch_MAg(matchId, customerId, betType, language, liveindicator, betteam);
            //}
            //else if (CurrentSession.IsSuper)
            //{
            //    runningBets = betListDAO.Age_BetList_SelRunningByBettypeMatch_SAg(matchId, customerId, betType, language, liveindicator, betteam);
            //}
            //else
            //{
            //    runningBets = new DataTable();
            //}
            runningBets = betListDAO.Age_BetList_TotalBet_SelRunningByBettypeMatch(matchId, customerId, betType, language, liveindicator, betteam);

            return runningBets;
        }

        /// <summary>
        /// Get Running bets by bettype and match for agent
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="liveindicator"></param>
        /// <param name="betteam"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetRunningByBettypeAndMatch(int matchId, int betType, int liveindicator, string betteam, out DataTable outDataTable)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            int customerId = CurrentSession.CustId;

            DataTable runningBets = new DataTable();
            //if (CurrentSession.IsAgent)
            //{
            //}
            //else if (CurrentSession.IsMaster)
            //{
            //    runningBets = betListDAO.Age_BetList_SelRunningByBettypeMatch_MAg(matchId, customerId, betType, language, liveindicator, betteam, out outDataTable);
            //}
            //else if (CurrentSession.IsSuper)
            //{
            //    runningBets = betListDAO.Age_BetList_SelRunningByBettypeMatch_SAg(matchId, customerId, betType, language, liveindicator, betteam, out outDataTable);
            //}
            //else
            //{
            //    runningBets = new DataTable();
            //    outDataTable = new DataTable();
            //}
            runningBets = betListDAO.Age_BetList_TotalBet_SelRunningByBettypeMatch(matchId, customerId, betType, language, liveindicator, betteam, out outDataTable);

            return runningBets;
        }

        #endregion RunningByBettypeAndMatch

        #region DailyStatement

        /// <summary>
        /// GetDailyStatement
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="transType"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetDailyStatement(int custId, DateTime dateFrom, int transType, out DataTable outDataTable)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;

            DataTable daillyBets = betListDAO.Age_BetList_SelByDayAll_Mem(custId, dateFrom, transType, language, out outDataTable);
            return daillyBets;
        }

        /// <summary>
        /// GetDailyStatement old from table 14
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetDailyStatementOld(int custId, DateTime dateFrom, out DataTable outDataTable)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;

            DataTable daillyBets = betListDAO.Age_BetList_SelByDayAll_Mem_Old(custId, dateFrom, language, out outDataTable);
            return daillyBets;
        }

        #endregion DailyStatement

        #region RunningBetByBetType

        /// <summary>
        /// Get running bets by bettype
        /// </summary>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable GetRunningByBettype(int betType)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            int customerId = CurrentSession.CustId;

            DataTable runningBets = new DataTable();

            runningBets = betListDAO.Acc_BetList_TotalBets_SelRunningByBettype(customerId, betType, language);

            return runningBets;
        }

        /// <summary>
        /// Get correct score bets
        /// </summary>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable GetRunningCorrectScore()
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            int customerId = CurrentSession.CustId;

            DataTable runningBets = null;
            runningBets = betListDAO.Acc_Betlist_TotalBets_Sel_4_30(customerId, language);

            return runningBets;
        }

        #endregion RunningBetByBetType

        #region ParlayWinloss

        /// <summary>
        /// Get parlay winloss
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable GetParlayWinloss(DateTime dateFrom, DateTime dateTo)
        {
            return GetParlayWinloss(dateFrom, dateTo, 9);
        }

        /// <summary>
        /// Get parlay winloss
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable GetParlayWinloss(DateTime dateFrom, DateTime dateTo, int bettype)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;
            int customerId = CurrentSession.CustId;

            DataTable mixParlay = null;
            if (CurrentSession.IsAgent)
            {
                mixParlay = betListDAO.Age_BetList_SelWinlossParlay_Ag(customerId, dateFrom, dateTo, language, Reports.ReportsBO.MoveBetsDate, bettype);
            }
            else if (CurrentSession.IsMaster)
            {
                mixParlay = betListDAO.Age_BetList_SelWinlossParlay_MAg(customerId, dateFrom, dateTo, language, Reports.ReportsBO.MoveBetsDate, bettype);
            }
            else if (CurrentSession.IsSuper)
            {
                mixParlay = betListDAO.Age_BetList_SelWinlossParlay_SAg(customerId, dateFrom, dateTo, language, Reports.ReportsBO.MoveBetsDate, bettype);
            }
            else
            {
                mixParlay = new DataTable();
            }

            return mixParlay;
        }

        #endregion ParlayWinloss

        #region Combination MixParlay

        /// <summary>
        /// Get Combination Mix Parlay
        /// </summary>
        /// <param name="transid"></param>
        /// <returns></returns>
        public DataTable GetCombinationMixParlay(long transid)
        {
            BetListDAO betListDAO = new BetListDAO();

            string language = CurrentSession.LangKey;

            DataTable mixParlay = new DataTable();
            mixParlay = betListDAO.Age_BetList_SelParlayDetail(transid, language);

            return mixParlay;
        }

        /// <summary>
        /// Get Combination Mix Parlay Detail
        /// </summary>
        /// <param name="refno"></param>
        /// <param name="winlostdate"></param>
        /// <returns></returns>
        public DbResult GetCombinationMixParlayDetail(long refno, long transid, long betid, int custId)
        {
            BetListDAO betListDAO = new BetListDAO();
            string language = CurrentSession.LangKey;

            return betListDAO.Acc_BetlistCombinationParlay_Details(refno, language, transid, betid, custId, ReportsBO.BalanceUpDate, ReportsBO.MoveBetsDate);
        }

        #endregion Combination MixParlay

        #region BetSlip

        public DataTable GetBetSlip(long transId)
        {
            BetListDAO dao = new BetListDAO();
            DataTable betSlip = new DataTable();
            betSlip = dao.Age_BettransSelBetSlip(transId, CurrentSession.LangKey);
            return betSlip;
        }

        public DataTable GetBetSlip_Old(long transId)
        {
            BetListDAO dao = new BetListDAO();
            DataTable betSlipOld = new DataTable();
            betSlipOld = dao.Age_BettransSelBetSlip_Old(transId, CurrentSession.LangKey);
            return betSlipOld;
        }

        #endregion BetSlip

        #region Cancel Betlist

        public DataTable GetCancelBetList_DeletedBets(int custId, int roleId)
        {
            BetListDAO betListDAO = new BetListDAO();
            DataTable betList = new DataTable();
            betList = betListDAO.Age_BetList_SelDeleted(custId, roleId, CurrentSession.LangKey);
            return betList;
        }

        public DataTable GetCancelBetList_DeletedBets(int custId, int roleId, out DataTable outDataTable)
        {
            BetListDAO betListDAO = new BetListDAO();
            DataTable betList = new DataTable();
            betList = betListDAO.Age_BetList_SelDeleted(custId, roleId, CurrentSession.LangKey, out outDataTable);
            return betList;
        }

        public DataTable GetCancelBetList_UserList(int custId, int roleId)
        {
            BetListDAO betListDAO = new BetListDAO();
            DataTable dt = null;

            if (((int)UserTypes.Agent).Equals(roleId))
            {
                dt = betListDAO.Age_BetList_SelDeleted_UserList_Ag(custId);
            }
            else if (((int)UserTypes.Master).Equals(roleId))
            {
                dt = betListDAO.Age_BetList_SelDeleted_UserList_MAg(custId);
            }
            else if (((int)UserTypes.Super).Equals(roleId))
            {
                dt = betListDAO.Age_BetList_SelDeleted_UserList_SAg(custId);
            }

            return dt;
        }

        #endregion Cancel Betlist

        #region WinlossByDateAndMem

        /// <summary>
        /// Report/WinLoss --> BetList
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="moveBetsDate"></param>
        /// <param name="filterParams"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable GetWinLossByDateAndMem(int custId, DateTime dateFrom, DateTime dateTo, DateTime moveBetsDate, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable outDataTable)
        {
            DataTable bettrans;
            BetListDAO betListDAO = new BetListDAO();
            string language = CurrentSession.LangKey;

            DateTime balanceUpDate = ReportsBO.BalanceUpDate;
            DateTime preBalanceUpDate = balanceUpDate.AddDays(-1);

            if (dateFrom < balanceUpDate)
            {
                if (dateTo < preBalanceUpDate)
                {
                    bettrans = betListDAO.Age_BetList_SelWinLossByDate_Mem_Old(custId, dateFrom, dateTo, language, filterParams, out outDataTable);
                }
                else
                {
                    // Separate 2 parts: dateFrom --> preBalanceUpDate, balanceUpDate --> dateTo
                    DataTable racing_bk = null;
                    DataTable bettrans_bk = betListDAO.Age_BetList_SelWinLossByDate_Mem_Old(custId, dateFrom, preBalanceUpDate, language, filterParams, out racing_bk);

                    DataTable racing = null;
                    DataTable bettrans_new = betListDAO.Age_BetList_SelWinLossByDate_Mem(custId, balanceUpDate, dateTo, language, ReportsBO.MoveBetsDate, filterParams, out racing);

                    // Merge two table bettrans
                    bettrans_bk = this.ConvertColumnsToLower(bettrans_bk);
                    bettrans_new = this.ConvertColumnsToLower(bettrans_new);
                    bettrans_bk.Merge(bettrans_new);
                    bettrans = bettrans_bk;

                    // Merge table racing
                    racing_bk.Merge(racing);
                    outDataTable = racing_bk;
                }
            }
            else
            {
                bettrans = betListDAO.Age_BetList_SelWinLossByDate_Mem(custId, dateFrom, dateTo, language, ReportsBO.MoveBetsDate, filterParams, out outDataTable);
            }

            return bettrans;
        }

        private DataTable ConvertColumnsToLower(DataTable dt)
        {
            DataTable table = new DataTable();
            DataRow newRow;
            foreach (DataColumn col in dt.Columns)
            {
                DataColumn column = new DataColumn(col.ColumnName.ToLower(), col.DataType);
                table.Columns.Add(column);
            }

            foreach (DataRow row in dt.Rows)
            {
                newRow = table.NewRow();
                foreach (DataColumn col in dt.Columns)
                {
                    if (!string.IsNullOrEmpty(row[col.ColumnName].ToString()))
                    {
                        newRow[col.ColumnName] = row[col.ColumnName].ToString();
                    }
                }

                table.Rows.Add(newRow);
            }

            return table;
        }

        #endregion WinlossByDateAndMem

        # region Agent Viewdetail

        public DataTable GetAgentPTComm(long transId)
        {
            BetListDAO dao = new BetListDAO();
            DataTable ptComm = new DataTable();
            //ptComm = dao.Acc_BetList_AgentDetail(transId, agentId, fromDate, toDate, moveBetsDay, showSB, showNG);
            ptComm = dao.Acc_BetList_TotalBet_AgentDetail(transId, CurrentSession.CustId);
            return ptComm;
        }

        #endregion
    }
}