/*
 * Created 20100507@Jacob - Business class of total bet function
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.Reports
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Caching;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.DL.Reports;
    using Sunplus.Agent.Model.Entities;

    public class ReportsBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        #region BalanceUpDate and MoveBetsDate
        private const string BALANCEUP_DAY_KEY = "SUNPLUS.AGENT.MODEL.BL.REPORTS.BALANCEUPDAY";

        public static DataTable GetBalanceUpDay()
        {
            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);

            object oldData = cache.Get<DataTable>(BALANCEUP_DAY_KEY);
            if (oldData == null)
            {
                ReportsDAO dao = new ReportsDAO();
                DataTable dt = dao.Age_SelBalanceUpDay();

                // Store in cache
                cache.Set<DataTable>(BALANCEUP_DAY_KEY, dt, DateTime.Now.AddMinutes(5));

                return dt;
            }

            return oldData as DataTable;
        }

        public static DateTime BalanceUpDate
        {
            get
            {
                return Convert.ToDateTime(GetBalanceUpDay().Rows[0]["BalanceUpDay"]);
            }
        }

        public static DateTime MoveBetsDate
        {
            get
            {
                return Convert.ToDateTime(GetBalanceUpDay().Rows[0]["MoveBetsDay"]);
            }
        }

        #endregion

        #region Constant
        /// <summary>
        /// Use this constant do config use bettrans usp on winloss report or not
        /// </summary>
        private const bool IGNOREED_BETTRANS = true;
        #endregion

        #region Varialbe
        private static readonly ReportsDAO reportDAO = new ReportsDAO();
        #endregion

        #region Constructor
        public ReportsBO(string netSessionId)
            : base(netSessionId)
        { 
        }
        #endregion

        #region Check role
        /// <summary>
        /// Prevent agent user the role of super and master.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private int CheckRoleIdConstrain(int roleId)
        {
            return roleId > CurrentSession.RoleId ? CurrentSession.RoleId : roleId;
        }
        #endregion

        #region MatchResultEntered
        public DataTable GetMatchResultEntered(out DateTime today, out DateTime yesterday)
        {
            DataTable reportData = reportDAO.Age_Rpt_MatchSelResultEntered();

            int numRow = reportData.Rows.Count;
            DataRow dtrYesterday = numRow > 0 ? reportData.Rows[0] : null;
            DataRow dtrToday = numRow > 1 ? reportData.Rows[1] : null;
            yesterday = numRow > 0 ? (DateTime)dtrYesterday["EventDate"] : DateTime.Today;
            today = numRow > 1 ? (DateTime)dtrToday["EventDate"] : DateTime.Today;

            DataTable result = new DataTable();
            result.Columns.Add("EventDate", typeof(string));
            result.Columns.Add("SportIndex", typeof(int));
            result.Columns.Add("SportType", typeof(string));
            result.Columns.Add("YesterdayEntered", typeof(int));
            result.Columns.Add("YesterdayNonEntered", typeof(int));
            if (numRow > 1)
            {
                result.Columns.Add("TodayEntered", typeof(int));
                result.Columns.Add("TodayNonEntered", typeof(int));
            }

            if (numRow > 0)
            {
                int totalEntered = 0, yesterdayEntered = 0;
                for (int i = 1, columnCount = reportData.Columns.Count - 1; i < columnCount; i += 2)
                {
                    DataRow dr = result.NewRow();
                    totalEntered = 0;
                    yesterdayEntered = 0;

                    dr["SportIndex"] = i;
                    dr["SportType"] = string.Empty;
                    dr["YesterdayEntered"] = dtrYesterday[i];
                    dr["YesterdayNonEntered"] = dtrYesterday[i + 1];
                    yesterdayEntered = Convert.ToInt32(dtrYesterday[i]) + Convert.ToInt32(dtrYesterday[i + 1]);

                    if (numRow > 1)
                    {
                        dr["TodayEntered"] = dtrToday[i];
                        dr["TodayNonEntered"] = dtrToday[i + 1];
                        totalEntered = Convert.ToInt32(dtrToday[i]) + Convert.ToInt32(dtrToday[i + 1]);
                    }

                    // Only add the non-empty rows
                    if (totalEntered > 0 || yesterdayEntered > 0)
                    {
                        result.Rows.Add(dr);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Return the table with 5 columns SportType, YesterDayEnter, YesterDayNonEnter, TodayEnter, TodayNonEnter
        /// </summary>
        /// <param name="today"></param>
        /// <returns></returns>
        public DataTable GetMatchResultEntered(out DateTime today)
        {
            var yesterday = DateTime.MinValue;
            today = DateTime.MinValue;

            var reportData = reportDAO.Age_Rpt_MatchSelResultEntered();
            var matchEnteredData = new Dictionary<int, DataRow>();

            // Convert to table
            var result = new DataTable();
            result.Columns.Add("SportType", typeof(int));
            result.Columns.Add("SportName", typeof(string));
            result.Columns.Add("YesterdayEntered", typeof(int));
            result.Columns.Add("YesterdayNonEntered", typeof(int));
            result.Columns.Add("TodayEntered", typeof(int));
            result.Columns.Add("TodayNonEntered", typeof(int));

            foreach (DataRow dr in reportData.Rows)
            {
                var isToday = (int)dr["IsToday"] == 1;
                var eventDate = (DateTime)dr["EventDate"];
                var entered = (int)dr["Entered"];
                var nonEntered = (int)dr["NonEntered"];
                var sportType = Convert.ToInt32(dr["SportType"].ToString());

                yesterday = isToday ? eventDate.AddDays(-1) : eventDate;
                today = isToday ? eventDate : eventDate.AddDays(1);

                var matchEntered = matchEnteredData.ContainsKey(sportType)
                                ? matchEnteredData[sportType]
                                : null;

                if (matchEntered == null)
                {
                    matchEntered = result.NewRow();
                    matchEntered["SportType"] = sportType;
                    matchEntered["SportName"] = Cache.CacheRef.GetSportName(sportType);
                    matchEntered["TodayEntered"] = 0;
                    matchEntered["TodayNonEntered"] = 0;
                    matchEntered["YesterdayEntered"] = 0;
                    matchEntered["YesterdayNonEntered"] = 0;

                    matchEnteredData.Add(sportType, matchEntered);
                }

                if (isToday)
                {
                    matchEntered["TodayEntered"] = entered;
                    matchEntered["TodayNonEntered"] = nonEntered;
                }
                else
                {
                    matchEntered["YesterdayEntered"] = entered;
                    matchEntered["YesterdayNonEntered"] = nonEntered;
                }
            }

            if (today == DateTime.MinValue)
            {
                today = yesterday == DateTime.MinValue ? DateTime.Today : yesterday.AddDays(1);
            }

            foreach (var key in matchEnteredData.Keys)
            {
                result.Rows.Add(matchEnteredData[key]);
            }

            return result;
        }

        #endregion

        #region GetWinloss

        /// <summary>
        /// Get winloss for super agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossSuper(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = reportDAO.Age_Rpt_WinLoss_SAg(customerId, fromDate, toDate, filterParams, BalanceUpDate);
            }
            else
            {
                winloss = reportDAO.Age_Rpt_WinLoss_SAg_bettrans(customerId, fromDate, toDate, filterParams);
            }

            return winloss;
        }

        /// <summary>
        /// Get winloss for master agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossMaster(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = reportDAO.Age_Rpt_WinLoss_MAg(customerId, fromDate, toDate, filterParams, BalanceUpDate);
            }
            else
            {
                winloss = reportDAO.Age_Rpt_WinLoss_MAg_bettrans(customerId, fromDate, toDate, filterParams);
            }

            return winloss;
        }

        /// <summary>
        /// Get winloss for agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossAgent(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = reportDAO.Age_Rpt_WinLoss_Ag(customerId, fromDate, toDate, filterParams, BalanceUpDate);
            }
            else
            {
                winloss = reportDAO.Age_Rpt_WinLoss_Ag_bettrans(customerId, fromDate, toDate, filterParams);
            }

            return winloss;
        }
        #endregion

        #region Get Winloss Analysis
        /// <summary>
        /// + @ReportType: 1-Daily; 2-Monthly; 3-Sports
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossAnalysisSuper(int customerId, DateTime fromDate, DateTime toDate, int reportType, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winlossAnalysis = null;
            winlossAnalysis = reportDAO.Age_Rpt_WinlossAnalysis_SAg(customerId, fromDate, toDate, reportType, filterParams, BalanceUpDate);
            return winlossAnalysis;
        }

        /// <summary>
        /// + @ReportType: 1-Daily; 2-Monthly; 3-Sports
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossAnalysisMaster(int customerId, DateTime fromDate, DateTime toDate, int reportType, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winlossAnalysis = null;
            winlossAnalysis = reportDAO.Age_Rpt_WinlossAnalysis_MAg(customerId, fromDate, toDate, reportType, filterParams, BalanceUpDate);
            return winlossAnalysis;
        }

        /// <summary>
        /// + @ReportType: 1-Daily; 2-Monthly; 3-Sports
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="reportType"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossAnalysisAgent(int customerId, DateTime fromDate, DateTime toDate, int reportType, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable winlossAnalysis = null;
            winlossAnalysis = reportDAO.Age_Rpt_WinlossAnalysis_Ag(customerId, fromDate, toDate, reportType, filterParams, BalanceUpDate);
            return winlossAnalysis;
        }

        #endregion
        
        #region Consolidated Reports

        public DataTable GetConsolidate(int custId, DateTime fromDate, DateTime toDate, int currency, int reportType, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DataTable consolidatedReport = null;
            consolidatedReport = reportDAO.Age_Rpt_ConsolidatedGroup(fromDate, toDate, custId, currency, reportType, filterParams, BalanceUpDate);
            return consolidatedReport;
        }

        public DataTable GetConsolidateCurrency(int custId)
        {
            DataTable consolidatedCurrency = null;
            consolidatedCurrency = reportDAO.Age_Rpt_ConsolidatedGroupSelCurrency(custId);
            return consolidatedCurrency;
        }

        #endregion

        #region GetWinlossDetail

        /// <summary>
        /// Get winloss detail for super agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossDetailSuper(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams, Hashtable pagingParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = reportDAO.Age_Rpt_WinLossDetail_SAg(fromDate, toDate, customerId, filterParams, BalanceUpDate, pagingParams);
            }
            else
            {
                winloss = reportDAO.Age_Rpt_WinLossDetail_SAg_bettrans(fromDate, toDate, customerId, filterParams, pagingParams);
            }

            return winloss;
        }

        /// <summary>
        /// Get winloss detail for master agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <param name="oldView"></param>
        /// <returns></returns>
        public DataTable GetWinlossDetailMaster(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams, bool oldView, Hashtable pagingParams)
        {
            DataTable winloss = null;
            if (oldView)
            {
                if (this.CheckConstraintMoveBetsDay(fromDate))
                {
                    winloss = reportDAO.Age_Rpt_WinLossDetail_MAg(fromDate, toDate, customerId, filterParams, BalanceUpDate, pagingParams);
                }
                else
                {
                    winloss = reportDAO.Age_Rpt_WinLossDetail_MAg_bettrans(fromDate, toDate, customerId, filterParams, pagingParams);
                }
            }
            else
            {
                if (this.CheckConstraintMoveBetsDay(fromDate))
                {
                    winloss = reportDAO.Age_Rpt_WinLossDetail_MAg_New(fromDate, toDate, customerId, filterParams, BalanceUpDate, pagingParams);
                }
                else
                {
                    winloss = reportDAO.Age_Rpt_WinLossDetail_MAg_bettrans_New(fromDate, toDate, customerId, filterParams, pagingParams);
                }
            }

            return winloss;
        }

        /// <summary>
        /// Get winloss detail for agent
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterParams"></param>
        /// <returns></returns>
        public DataTable GetWinlossDetailAgent(int customerId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams, Hashtable pagingParams)
        {
            DataTable winloss = null;
            if (this.CheckConstraintMoveBetsDay(fromDate))
            {
                winloss = reportDAO.Age_Rpt_WinLossDetail_Ag(fromDate, toDate, customerId, filterParams, BalanceUpDate, pagingParams);
            }
            else
            {
                winloss = reportDAO.Age_Rpt_WinLossDetail_Ag_bettrans(fromDate, toDate, customerId, filterParams, pagingParams);
            }

            return winloss;
        }

        #endregion

        #region Winloss Helper
        /// <summary>
        /// Check constraint between Lower Bound Date and Move Bet Date
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        private bool CheckConstraintMoveBetsDay(DateTime fromDate)
        {
            // isIgnoredBettrans=true : ignored constraint
            return IGNOREED_BETTRANS;// || DateTime.Compare(fromDate, MoveBetsDate) <= 0;
        }
        #endregion

        #region Outstanding
        /// <summary>
        /// Get member outstanding
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable GetMemberOutstanding(int custId, int roleId)
        {
            DataTable outstanding = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (CurrentSession.IsAgent || roleId == 2)
            {
                outstanding = reportDAO.Age_Rpt_Outstanding_Ag(custId);
            }
            else if (CurrentSession.IsMaster || roleId == 3)
            {
                outstanding = reportDAO.Age_Rpt_Outstanding_MAg(custId);
            }
            else if (CurrentSession.IsSuper)
            {
                outstanding = reportDAO.Age_Rpt_Outstanding_SAg(custId);
            }
            else
            {
                outstanding = new DataTable();
            }

            return outstanding;
        }
        #endregion

        #region MatchWinloss
        /// <summary>
        /// Get match winloss detail
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetMatchWinloss(DateTime dateFrom, DateTime dateTo)
        {
            DataTable winloss = null;

            int customerID = CurrentSession.CustId;

            // execute
            if (CurrentSession.IsAgent)
            {
                winloss = reportDAO.Age_Rpt_MatchWinLoss_Ag(customerID, dateFrom, dateTo);
            }
            else
            {
                winloss = new DataTable();
            }

            return winloss;
        }
        #endregion

        #region MatchWinlossDetail
        /// <summary>
        /// Get Leagues by Sport
        /// </summary>
        /// <returns></returns>
        public DataTable GetLeagueBySport(DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            DateTime moveBetsDate = MoveBetsDate;
            string language = CurrentSession.LangKey;
            return reportDAO.Age_Rpt_MatchWinloss_SelLeagueBySport(CurrentSession.CustId, fromDate, toDate, filterParams, language, moveBetsDate);
        }

        /// <summary>
        /// Get match winloss detail
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetMatchWinlossDetail(DateTime dateFrom, DateTime dateTo, ICollection<KeyValuePair<string, bool>> filterParams, string league)
        {
            DataTable winlossDetail = null;

            int customerID = CurrentSession.CustId;
            DateTime moveBetsDate = MoveBetsDate;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                winlossDetail = reportDAO.Age_Rpt_MatchWinLossDetail_Ag(customerID, dateFrom, dateTo, filterParams, league, language, moveBetsDate);
            }
            else if (CurrentSession.IsMaster)
            { 
                winlossDetail = reportDAO.Age_Rpt_MatchWinLossDetail_MAg(customerID, dateFrom, dateTo, filterParams, league, language, moveBetsDate); 
            }
            else if (CurrentSession.IsSuper)
            {
                winlossDetail = reportDAO.Age_Rpt_MatchWinLossDetail_SAg(customerID, dateFrom, dateTo, filterParams, league, language, moveBetsDate);
            }
            else
            {
                winlossDetail = new DataTable();
            }

            return winlossDetail;
        }
        #endregion

        #region WinlossByBetType
        /// <summary>
        /// Get winloss by bettype
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetWinlossByBetType(DateTime dateFrom, DateTime dateTo)
        {
            DataTable winloss = null;

            int customerID = CurrentSession.CustId;
            DateTime balanceupDate = BalanceUpDate;

            // execute
            if (CurrentSession.IsAgent)
            {
                winloss = reportDAO.Age_Rpt_WinLossByBettype_Ag(customerID, dateFrom, dateTo, balanceupDate);
            }
            else if (CurrentSession.IsMaster)
            {
                winloss = reportDAO.Age_Rpt_WinLossByBettype_MAg(customerID, dateFrom, dateTo, balanceupDate);
            }
            else if (CurrentSession.IsSuper)
            {
                winloss = reportDAO.Age_Rpt_WinLossByBettype_SAg(customerID, dateFrom, dateTo, balanceupDate);
            }
            else
            {
                winloss = new DataTable();
            }

            return winloss;
        }
        #endregion

        #region CommisionByBetType
        /// <summary>
        /// Get winloss by bettype
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetCommisionByBetType(int custId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            DataTable commision = null;
            roleId = this.CheckRoleIdConstrain(roleId);
            DateTime moveBetDay = MoveBetsDate;

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                commision = reportDAO.Age_Rpt_CommByBettype_Ag(custId, dateFrom, dateTo, moveBetDay);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                commision = reportDAO.Age_Rpt_CommByBettype_MAg(custId, dateFrom, dateTo, moveBetDay);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                commision = reportDAO.Age_Rpt_CommByBettype_SAg(custId, dateFrom, dateTo, moveBetDay);
            }
            else
            {
                commision = new DataTable();
            }

            return commision;
        }
        #endregion

        #region FinanceCommision
        /// <summary>
        /// Get finance commission
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetFinanceCommision(int custId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            DataTable commision = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                commision = reportDAO.Age_Rpt_CommByFinance_Ag(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                commision = reportDAO.Age_Rpt_CommByFinance_MAg(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                commision = reportDAO.Age_Rpt_CommByFinance_SAg(custId, dateFrom, dateTo);
            }
            else
            {
                commision = new DataTable();
            }

            return commision;
        }
        #endregion

        #region FinanceCommision
        /// <summary>
        /// Get HR commission
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetHRCommision(int custId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            DataTable commision = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                commision = reportDAO.Age_Rpt_CommByBettype_HR_Ag(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                commision = reportDAO.Age_Rpt_CommByBettype_HR_MAg(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                commision = reportDAO.Age_Rpt_CommByBettype_HR_SAg(custId, dateFrom, dateTo);
            }
            else
            {
                commision = new DataTable();
            }

            return commision;
        }
        #endregion

        #region DailyStatment

        /// <summary>
        /// Get daily statement
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="pagingParams"></param>
        /// <returns></returns>
        public DataTable GetDailyStatement(int custId, int roleId, DateTime dateFrom, DateTime dateTo, Hashtable pagingParams)
        {
            DataTable statement = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                statement = reportDAO.Age_Statement_Rpt_Ag(custId, dateFrom, dateTo, BalanceUpDate, pagingParams);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                statement = reportDAO.Age_Statement_Rpt_MAg(custId, dateFrom, dateTo, BalanceUpDate, pagingParams);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                statement = reportDAO.Age_Statement_Rpt_SAg(custId, dateFrom, dateTo, BalanceUpDate, pagingParams);
            }
            else
            {
                statement = new DataTable();
            }

            return statement;
        }

        /// <summary>
        /// Get daily statement
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetDailyStatement(int custId, int roleId, DateTime dateFrom, DateTime dateTo)
        {
            DataTable statement = null;
            roleId = this.CheckRoleIdConstrain(roleId);

            // execute
            if (roleId == (int)UserTypes.Agent)
            {
                statement = reportDAO.Age_Statement_Rpt_Ag(custId, dateFrom, dateTo);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                statement = reportDAO.Age_Statement_Rpt_MAg(custId);
            }
            else if (roleId == (int)UserTypes.Super)
            {
                statement = reportDAO.Age_Statement_Rpt_SAg(custId);
            }
            else
            {
                statement = new DataTable();
            }

            return statement;
        }
        #endregion

        #region OutrightResult
        /// <summary>
        /// Get outright result
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public DataTable GetOutrightResult(DateTime dateFrom, DateTime dateTo)
        {
            return reportDAO.Age_Result_Outright(dateFrom, dateTo);
        }
        #endregion

        #region SportListByDate
        /// <summary>
        /// Get sport list by date
        /// Show to sport dropdown list in report result
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetResultSportByDate(DateTime date)
        {
            return reportDAO.Age_Result_SportSelByDate(date);
        }
        #endregion

        #region ResultReport
        /// <summary>
        /// Get result of match
        /// </summary>
        /// <param name="date"></param>
        /// <param name="sportId"></param>
        /// <param name="leaugeId"></param>
        /// <returns></returns>
        public DataTable GetResultMatchByLeague(DateTime date, int sportId, int leagueId)
        {
            string language = CurrentSession.Language;
            return reportDAO.Age_Result_MatchSelByLeague(date, CurrentSession.CustId, sportId, leagueId, language);
        }

        /// <summary>
        /// Get result for number game
        /// </summary>
        /// <param name="date"></param>
        /// <param name="leaugeId"></param>
        /// <returns></returns>
        public DataTable GetResultNumberGameByLeague(DateTime date, int leagueId)
        {
            string language = CurrentSession.Language;
            return reportDAO.Age_Result_MatchSelNumberGameByLeague(date, leagueId, language);
        }

        /// <summary>
        /// Get result of horse racing
        /// </summary>
        /// <param name="date"></param>
        /// <param name="leagueId"></param>
        /// <returns></returns>
        public DataTable GetResultHRRaces(DateTime date, int sportId, int leagueId)
        {
            return reportDAO.Age_Result_RaceSelByMeeting(date, sportId, leagueId);
        }

        /// <summary>
        /// Get match result of number game
        /// </summary>
        /// <param name="matchId"></param>
        /// <returns></returns>
        public DataTable SelectMatchForViewNumberGameResult(int matchId)
        {
            return reportDAO.Age_BetList_SelMatchResultNumberGame(matchId);
        }

        /// <summary>
        /// Get match result by matchId and race number
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="raceno"></param>
        /// <returns></returns>
        public DataTable SelectMatchForViewResult(int matchId, string raceno)
        {
            return reportDAO.Age_BetList_SelMatchResult(matchId, raceno);
        }

        /// <summary>
        /// Get match result Sporting Solutions
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="raceno"></param>
        /// <returns></returns>
        public DataTable SelectSetDetailForViewResult(int matchId, string resourceId)
        {
            return reportDAO.Age_BetList_SelMatchResultSS(matchId, resourceId);
        }

        #endregion

        #region GetLeagueByDate
        /// <summary>
        /// Get all leagues by date
        /// Show to league dropdown list in report result
        /// </summary>
        /// <param name="date"></param>
        /// <param name="sportId"></param>
        /// <returns></returns>
        public DataTable GetResultLeagueByDate(DateTime date, int sportId)
        {
            string language = CurrentSession.Language;

            // Racing
            if (sportId == 151)
            {
                if (!AppSettings.Disable151_31 && CurrentSession.IsAllowHRCurrency)
                {
                    return reportDAO.Age_Result_LeagueSelByDate(date, sportId, language);
                }
                else
                {
                    return new DataTable();
                }
            }
            else
            {
                return reportDAO.Age_Result_LeagueSelByDate(date, sportId, language);
            }
        }

        #endregion

        #region For Last Bets Monitor
        public DataTable GetLastBetList(int sportTypeGroup, int betTypeGroup, long transId, int limitRowCount)
        {
            int custId = CurrentSession.CustId;
            int roleId = CurrentSession.RoleId;

            DataTable dtbResult = null;

            switch (sportTypeGroup)
            {
                case (int)SportType.None: // All sports
                    switch (betTypeGroup)
                    {
                        case (int)BetType.None: // All bet types
                            switch (roleId)
                            {
                                case (int)UserTypes.Agent:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_Ag_AllBettypes(custId, transId, limitRowCount);
                                    break;
                                case (int)UserTypes.Master:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_MAg_AllBettypes(custId, transId, limitRowCount);
                                    break;
                                case (int)UserTypes.Super:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_SAg_AllBettypes(custId, transId, limitRowCount);
                                    break;
                            }

                            break;
                        case (int)BetType.Handicap:
                            switch (roleId)
                            {
                                case (int)UserTypes.Agent:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_Ag_HDP(custId, transId, limitRowCount);
                                    break;
                                case (int)UserTypes.Master:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_MAg_HDP(custId, transId, limitRowCount);
                                    break;
                                case (int)UserTypes.Super:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_SAg_HDP(custId, transId, limitRowCount);
                                    break;
                            }

                            break;
                        case (int)BetType.Over_Under:
                            switch (roleId)
                            {
                                case (int)UserTypes.Agent:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_Ag_OU(custId, transId, limitRowCount);
                                    break;
                                case (int)UserTypes.Master:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_MAg_OU(custId, transId, limitRowCount);
                                    break;
                                case (int)UserTypes.Super:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_SAg_OU(custId, transId, limitRowCount);
                                    break;
                            }

                            break;
                        case (int)BetType.Others: // Other bet types
                            switch (roleId)
                            {
                                case (int)UserTypes.Agent:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_Ag_OtherBettypes(custId, transId, limitRowCount);
                                    break;
                                case (int)UserTypes.Master:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_MAg_OtherBettypes(custId, transId, limitRowCount);
                                    break;
                                case (int)UserTypes.Super:
                                    dtbResult = reportDAO.Age_Monitor_BetlistSel_SAg_OtherBettypes(custId, transId, limitRowCount);
                                    break;
                            }

                            break;
                        default:
                            dtbResult = new DataTable();
                            break;
                    }

                    break;
                default:
                    dtbResult = new DataTable();
                    break;
            }

            // Load match from cache (key: matchid) and fill (matchcode, sporttype, leagueid, homeid, awayid, race) in dtResult
            dtbResult.Columns.Add("showtime", typeof(string));
            DataRowCollection rows = dtbResult.Rows;
            
            foreach (DataRow row in rows)
            {
                int betType = Convert.ToInt32(row["bettype"]);
                Hashtable matches = GetRunningMatch(false, betType);

                int matchId = (int)row["matchid"];
                DataRow matchRow = matches[matchId] as DataRow;

                // Min match id, in case someone hard code match id for other purpose
                if (null == matchRow && matchId > 999) 
                {
                    matches = GetRunningMatch(true, betType);
                    matchRow = matches[matchId] as DataRow;
                }

                if (null == matchRow)
                {
                    continue;
                }

                row["matchcode"] = matchRow["matchcode"];
                row["sporttype"] = matchRow["sporttype"];
                row["leagueid"] = matchRow["leagueid"];
                row["homeid"] = matchRow["homeid"];
                row["awayid"] = matchRow["awayid"];
                row["race"] = matchRow["race"];
                row["showtime"] = matchRow.Table.Columns.Contains("showtime") ? matchRow["showtime"] : string.Empty;
            }

            return dtbResult;
        }
      
        private const string RUNNINGMATCH_KEY = "SUNPLUS.AGENT.MODEL.BL.REPORTS.RUNNINGMATCH";
        private const string OUTRIGHTMATCH_KEY = "SUNPLUS.AGENT.MODEL.BL.REPORTS.OUTRIGHTMATCH";
        private static bool isGetRunningMatch = false;
        private static object getRunningMatchLocker = new object();

        /// <summary>
        /// matchid is key, value is DataRow. At one time, just one GetRunningMatch was called.
        /// </summary>
        /// <param name="refreshCache"></param>
        /// <returns></returns>
        private static Hashtable GetRunningMatch(bool refreshCache, int betType)
        {
            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);

            bool isOutright = betType == 10 || betType == 31 || betType == 32 || betType == 33;
            string cacheName = isOutright ? OUTRIGHTMATCH_KEY : RUNNINGMATCH_KEY;

            object oldData = cache.Get<Hashtable>(cacheName);

            if ((null == oldData || refreshCache) && !isGetRunningMatch)
            {
                lock (getRunningMatchLocker)
                {
                    if (!isGetRunningMatch)
                    {
                        isGetRunningMatch = true;

                        DataTable dtbMatch = isOutright ? reportDAO.Age_Monitor_OutrightSel() : reportDAO.Age_Monitor_MatchSel(0);

                        DataRowCollection rows = dtbMatch.Rows;
                        Hashtable hash = new Hashtable(rows.Count);
                        foreach (DataRow row in rows)
                        {
                            hash.Add((int)row["matchid"], row);
                        }

                        cache.Set<Hashtable>(cacheName, hash, DateTime.Now.AddHours(1));

                        isGetRunningMatch = false;

                        return hash;
                    }
                }
            }
            else if (null == oldData)
            {
                return new Hashtable();
            }

            return oldData as Hashtable;
        }
        #endregion

        #region Cancel Betlist

        public DataTable BetList_SelDeleted(Hashtable htb)
        {
            return reportDAO.BetList_SelDeleted(htb);
        }

        public DataTable CancelBetList_Agent(string custId)
        {
            return reportDAO.CancelBetList_Agent(custId);            
        }

        public DataTable CancelBetList_Master(string custId)
        {
            return reportDAO.CancelBetList_Master(custId);
        }

        public DataTable CancelBetList_Super(string custId)
        {
            return reportDAO.CancelBetList_Super(custId);
        }         
        #endregion

        #region Portal Page

        #region Balance Info
        public DataTable GetBalanceInfo(int custId)
        {
            var date = MoveBetsDate;

            if (UserSession.Current.IsAgent)
            {
                return reportDAO.Age_CustomerSelBalanceInfo_Ag(custId, date);
            }
            else if (UserSession.Current.IsMaster)
            {
                return reportDAO.Age_CustomerSelBalanceInfo_MAg(custId, date);
            }
            else if (UserSession.Current.IsSuper)
            {
                return reportDAO.Age_CustomerSelBalanceInfo_SAg(custId, date);
            }

            return null;
        }
        #endregion

        #region Statistic Info
        public DataTable GetStatisticInfo(int custId)
        {
            if (UserSession.Current.IsAgent)
            {
                return reportDAO.Age_CustomerSelStatisticsInfo_Ag(custId);
            }
            else if (UserSession.Current.IsMaster)
            {
                return reportDAO.Age_CustomerSelStatisticsInfo_MAg(custId);
            }
            else if (UserSession.Current.IsSuper)
            {
                return reportDAO.Age_CustomerSelStatisticsInfo_SAg(custId);
            }

            return null;
        }

        public DataTable GetNewMembers(int custId)
        {
            return reportDAO.Age_CustomerSelNewMembers(custId);            
        }

        public DataTable GetTopWinningMembers(int custId)
        {
            if (UserSession.Current.IsAgent)
            {
                return reportDAO.Age_CustomerSelTopWinningMembers_Ag(custId, BalanceUpDate, MoveBetsDate);
            }
            else if (UserSession.Current.IsMaster)
            {
                return reportDAO.Age_CustomerSelTopWinningMembers_MAg(custId, BalanceUpDate, MoveBetsDate);
            }
            else if (UserSession.Current.IsSuper)
            {
                return reportDAO.Age_CustomerSelTopWinningMembers_SAg(custId, BalanceUpDate, MoveBetsDate);
            }

            return null;
        }

        public DataTable GetTopTurnoverMembers(int custId)
        {
            if (UserSession.Current.IsAgent)
            {
                return reportDAO.Age_CustomerSelTopTurnoverMembers_Ag(custId, BalanceUpDate, MoveBetsDate);
            }
            else if (UserSession.Current.IsMaster)
            {
                return reportDAO.Age_CustomerSelTopTurnoverMembers_MAg(custId, BalanceUpDate, MoveBetsDate);
            }
            else if (UserSession.Current.IsSuper)
            {
                return reportDAO.Age_CustomerSelTopTurnoverMembers_SAg(custId, BalanceUpDate, MoveBetsDate);
            }

            return null;
        }
        #endregion

        #region Online List
        public DataTable GetOnlineList(int custId)
        {
            if (UserSession.Current.IsAgent)
            {
                return reportDAO.Age_CustomerSelOnlineList_Ag(custId);
            }
            else if (UserSession.Current.IsMaster)
            {
                return reportDAO.Age_CustomerSelOnlineList_MAg(custId);
            }
            else if (UserSession.Current.IsSuper)
            {
                return reportDAO.Age_CustomerSelOnlineList_SAg(custId);
            }
            
            return null;
        }
        #endregion

        #endregion

        #region MixParlay

        /// <summary>
        /// Select matches from BetTransM by Refno
        /// For view MixParlay result
        /// </summary>        
        public DataTable SelectMatchResultParlay(long refno)
        {
            return reportDAO.Age_BetList_SelMatchResultParlay(refno);            
        }

        #endregion
    }
}
