using System.Data;

/*
 * Created 20100506@Jacob - Data access object of BetList function
 * Revision:
 * 20110301@Qui - Refactor
 *
 */

namespace Sunplus.Agent.Model.DL.BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Data.DNAide;

    public class BetListDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        #region GetWinLossByDateAndMem

        /// <summary>
        /// Report/WinLoss --> BetList
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <param name="filterParams"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinLossByDate_Mem(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);

            foreach (KeyValuePair<string, bool> element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            DataSet result = ExecSpDataset("Age_BetList_SelWinLossByDate_Mem", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelWinLossByDate_Mem", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Report/WinLoss --> BetList
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="filterParams"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinLossByDate_Mem_Old(int custId, DateTime dateFrom, DateTime dateTo, string language, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);

            foreach (KeyValuePair<string, bool> element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            DataSet result = ExecSpDataset("Age_BetList_SelWinLossByDate_Mem_Old", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelWinLossByDate_Mem_Old", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion GetWinLossByDateAndMem

        #region WinlossOutright

        /// <summary>
        /// BetList -> Get winloss outright
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossOutright_Ag(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);

            return ExecSpTable("Age_BetList_SelWinlossOutright_Ag", parameters);
        }

        /// <summary>
        /// BetList -> Get winloss outright
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossOutright_MAg(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);

            return ExecSpTable("Age_BetList_SelWinlossOutright_MAg", parameters);
        }

        /// <summary>
        /// BetList -> Get winloss outright
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossOutright_SAg(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);

            return ExecSpTable("Age_BetList_SelWinlossOutright_SAg", parameters);
        }

        #endregion WinlossOutright

        #region WinlossHR

        /// <summary>
        /// Get agent winloss in horse racing
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="leagueId"></param>
        /// <param name="race"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossHR_Ag(int custId, int betType, int leagueId, string race, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("leagueid", leagueId);
            parameters.Add("race", race);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);

            // return ExecSpTable("Age_BetList_SelWinlossHR_Ag", parameters);
            DataSet result = ExecSpDataset("Age_BetList_SelWinlossHR_Ag", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelWinlossHR_Ag", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get master winloss in horse racing
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="leagueId"></param>
        /// <param name="race"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossHR_MAg(int custId, int betType, int leagueId, string race, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("leagueid", leagueId);
            parameters.Add("race", race);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);

            // return ExecSpTable("Age_BetList_SelWinlossHR_MAg", parameters);
            DataSet result = ExecSpDataset("Age_BetList_SelWinlossHR_MAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelWinlossHR_MAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get super winloss in horse racing
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="leagueId"></param>
        /// <param name="race"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossHR_SAg(int custId, int betType, int leagueId, string race, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("leagueid", leagueId);
            parameters.Add("race", race);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);

            // return ExecSpTable("Age_BetList_SelWinlossHR_SAg", parameters);
            DataSet result = ExecSpDataset("Age_BetList_SelWinlossHR_SAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelWinlossHR_SAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion WinlossHR

        #region WinlossByBetTypeMatch

        /// <summary>
        /// Get agent Winloss by match
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossByBettypeMatch_Ag(int custId, int matchId, int betType, string language, string userName, int liveindicator, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("username", userName);
            parameters.Add("liveindicator", liveindicator);

            DataSet result = ExecSpDataset("Age_BetList_SelWinlossByBettypeMatch_Ag", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelWinlossByBettypeMatch_Ag", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get master Winloss by match
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossByBettypeMatch_MAg(int custId, int matchId, int betType, string language, string userName, int liveindicator, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("username", userName);
            parameters.Add("liveindicator", liveindicator);

            DataSet result = ExecSpDataset("Age_BetList_SelWinlossByBettypeMatch_MAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelWinlossByBettypeMatch_MAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get super Winloss by match
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossByBettypeMatch_SAg(int custId, int matchId, int betType, string language, string userName, int liveindicator, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("username", userName);
            parameters.Add("liveindicator", liveindicator);

            DataSet result = ExecSpDataset("Age_BetList_SelWinlossByBettypeMatch_SAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelWinlossByBettypeMatch_SAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion WinlossByBetTypeMatch

        #region RunningByDateAndMem

        public DataTable Age_BetList_SelRunningByDate_Mem(int custId, string userName, DateTime dateFrom, DateTime dateTo, string language, int getAll, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable dataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("getall", getAll);
            parameters.Add("language", language);
            parameters.Add("username", userName);

            foreach (KeyValuePair<string, bool> element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            DataSet result = ExecSpDataset("Age_BetList_SelRunningByDate_Mem", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                dataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelRunningByDate_Mem", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion RunningByDateAndMem

        #region AllBetsByDateAndMem

        public DataTable Age_BetList_SelAllByDate_Mem(int custId, int agentId, string userName, DateTime dateFrom, DateTime dateTo, string language, ICollection<KeyValuePair<string, bool>> filterParams, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("agid", agentId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("username", userName);

            foreach (KeyValuePair<string, bool> element in filterParams)
            {
                parameters.Add(element.Key, element.Value);
            }

            DataSet result = ExecSpDataset("Age_BetList_SelAllByDate_Mem", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelAllByDate_Mem", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion AllBetsByDateAndMem

        #region DeletedBets

        /// <summary>
        /// Get deleted bets
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelDeleted(int custId, int roleId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("roleid", roleId);
            parameters.Add("language", language);
            return ExecSpTable("Age_BetList_SelDeleted", parameters);
        }

        /// <summary>
        /// Get deleted bets
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="language"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelDeleted(int custId, int roleId, string language, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("roleid", roleId);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelDeleted", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelDeleted", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion DeletedBets

        #region AllHRByDate

        /// <summary>
        /// Get horse racing bet by matchid for agent
        /// @HanhVu: Select data for Betlist of HR (link from Total Bets==>HR)
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="matchId"></param>
        /// <param name="language"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelAllByDateHR_Ag(int custId, int betType, int matchId, string language, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("matchid", matchId);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelAllByDateHR_Ag", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelAllByDateHR_Ag", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get horse racing bet by matchid for master
        /// @HanhVu: Select data for Betlist of HR (link from Total Bets==>HR)
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="matchId"></param>
        /// <param name="language"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelAllByDateHR_MAg(int custId, int betType, int matchId, string language, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("matchid", matchId);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelAllByDateHR_MAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelAllByDateHR_MAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get horse racing bet by matchid for super
        /// @HanhVu: Select data for Betlist of HR (link from Total Bets==>HR)
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="matchId"></param>
        /// <param name="language"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelAllByDateHR_SAg(int custId, int betType, int matchId, string language, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("matchid", matchId);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelAllByDateHR_SAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelAllByDateHR_SAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion AllHRByDate

        #region RunningByBettypeAndMatch

        /// <summary>
        /// Get Running bets by bettype and match for agent
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_Ag(int matchId, int custId, int betType, string language, int liveindicator, string betteam)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            return ExecSpTable("Age_BetList_SelRunningByBettypeMatch_Ag", parameters);
        }

        /// <summary>
        /// Get Running bets by bettype and match for agent
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="liveindicator"></param>
        /// <param name="betteam"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_Ag(int matchId, int custId, int betType, string language, int liveindicator, string betteam, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            DataSet result = ExecSpDataset("Age_BetList_SelRunningByBettypeMatch_Ag", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelRunningByBettypeMatch_Ag", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get Running bets by bettype and match for master
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_MAg(int matchId, int custId, int betType, string language, int liveindicator, string betteam)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            return ExecSpTable("Age_BetList_SelRunningByBettypeMatch_MAg", parameters);
        }

        /// <summary>
        /// Get Running bets by bettype and match for master
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="liveindicator"></param>
        /// <param name="betteam"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_MAg(int matchId, int custId, int betType, string language, int liveindicator, string betteam, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            DataSet result = ExecSpDataset("Age_BetList_SelRunningByBettypeMatch_MAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelRunningByBettypeMatch_MAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get Running bets by bettype and match for super
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_SAg(int matchId, int custId, int betType, string language, int liveindicator, string betteam)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);
            return ExecSpTable("Age_BetList_SelRunningByBettypeMatch_SAg", parameters);
        }

        /// <summary>
        /// Get Running bets by bettype and match for super
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <param name="liveindicator"></param>
        /// <param name="betteam"></param>
        /// <param name="outDataTable"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettypeMatch_SAg(int matchId, int custId, int betType, string language, int liveindicator, string betteam, out DataTable outDataTable)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);
            parameters.Add("liveindicator", liveindicator);
            parameters.Add("betteam", betteam);

            DataSet result = ExecSpDataset("Age_BetList_SelRunningByBettypeMatch_SAg", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelRunningByBettypeMatch_SAg", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion RunningByBettypeAndMatch

        #region DailyStatement

        /// <summary>
        /// Get dailly statement
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="transType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelByDayAll_Mem(int custId, DateTime dateFrom, int transType, string language, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("transtype", transType);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelByDayAll_Mem", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelByDayAll_Mem", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        /// <summary>
        /// Get dailly statement old data from table 14
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelByDayAll_Mem_Old(int custId, DateTime dateFrom, string language, out DataTable outDataTable)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("language", language);

            DataSet result = ExecSpDataset("Age_BetList_SelByDayAll_Mem_Old", parameters);

            // Check exist a DataTable in DataSet
            if (result.Tables.Count > 0)
            {
                outDataTable = result.Tables.Count > 1 ? result.Tables[01] : new DataTable();
                return result.Tables[0];
            }
            else
            {
                // Throw DataException and log the SP calling
                DataException error = new DataException("Age_BetList_SelByDayAll_Mem_Old", parameters);
                Sunplus.Agent.Utilities.Logs.Write(error.Message);
                throw error;
            }
        }

        #endregion DailyStatement

        #region RunningByBettype

        /// <summary>
        /// Get running bets by bet type for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettype_Ag(int custId, int betType, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);

            return ExecSpTable("Age_BetList_SelRunningByBettype_Ag", parameters);
        }

        /// <summary>
        /// Get running bets by bet type for master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettype_MAg(int custId, int betType, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);

            return ExecSpTable("Age_BetList_SelRunningByBettype_MAg", parameters);
        }

        /// <summary>
        /// Get running bets by bet type for super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelRunningByBettype_SAg(int custId, int betType, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("language", language);

            return ExecSpTable("Age_BetList_SelRunningByBettype_SAg", parameters);
        }

        /// <summary>
        /// Get correct score for Agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Acc_Betlist_TotalBets_Sel_4_30(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Acc_Betlist_TotalBets_Sel_4_30", parameters);
        }

        #endregion RunningByBettype

        #region ParlayWinloss

        /// <summary>
        /// Details for MixParlay for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossParlay_Ag(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int bettype)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);
            parameters.Add("bettype", bettype);
            return ExecSpTable("Age_BetList_SelWinlossParlay_Ag", parameters);
        }

        /// <summary>
        /// Details for MixParlay for master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossParlay_MAg(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int bettype)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);
            parameters.Add("bettype", bettype);
            return ExecSpTable("Age_BetList_SelWinlossParlay_MAg", parameters);
        }

        /// <summary>
        /// Details for MixParlay for super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="moveBetsDate"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelWinlossParlay_SAg(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime moveBetsDate, int bettype)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("movebetsday", moveBetsDate);
            parameters.Add("bettype", bettype);
            return ExecSpTable("Age_BetList_SelWinlossParlay_SAg", parameters);
        }

        #endregion ParlayWinloss

        #region MixParlay

        /// <summary>
        /// Get Mix Parlay BetList
        /// </summary>
        /// <param name="transid"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_BetList_SelParlayDetail(long transid, string language)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("transid", transid);
            parameters.Add("language", language);

            return ExecSpTable("Age_BetList_SelParlayDetail", parameters);
        }

        /// <summary>
        /// Get Combination Mix Parlay BetList Detail
        /// </summary>
        /// <param name="refno"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DbResult Acc_BetlistCombinationParlay_Details(long refno, string language, long transid, long betid, int custId, DateTime balanceupday, DateTime movebetday)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("refno", refno);
            parameters.Add("language", language);
            parameters.Add("transid", transid);
            parameters.Add("betid", betid);
            parameters.Add("custid", custId);
            parameters.Add("movebetday", movebetday);
            parameters.Add("balanceupday", balanceupday);

            return ExecSp("Acc_BetlistCombinationParlay_Details", parameters);
        }

        #endregion MixParlay

        #region BetSlip

        public DataTable Age_BettransSelBetSlip(long transId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("transid", transId);
            parameters.Add("language", language);
            return ExecSpTable("Age_BettransSelBetSlip", parameters);
        }

        public DataTable Age_BettransSelBetSlip_Old(long transId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("transid", transId);
            parameters.Add("language", language);
            return ExecSpTable("Age_BettransSelBetSlip_Old", parameters);
        }

        #endregion BetSlip

        #region Cancel Betlist

        public DataTable Age_BetList_SelDeleted_UserList_Ag(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            return ExecSpTable("Age_BetList_SelDeleted_UserList_Ag", parameters);
        }

        public DataTable Age_BetList_SelDeleted_UserList_MAg(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            return ExecSpTable("Age_BetList_SelDeleted_UserList_MAg", parameters);
        }

        public DataTable Age_BetList_SelDeleted_UserList_SAg(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            return ExecSpTable("Age_BetList_SelDeleted_UserList_SAg", parameters);
        }

        #endregion Cancel Betlist

        # region Agent Viewdetail

        public DataTable Acc_BetList_AgentDetail(long transId, long agentId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, bool showSB, bool showNG)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("transid", transId);
            parameters.Add("agentid", agentId);
            parameters.Add("fromdate", fromDate);
            parameters.Add("todate", toDate);
            parameters.Add("movebetsday", moveBetsDay);
            parameters.Add("showsb", showSB);
            parameters.Add("showng", showNG);
            return ExecSpTable("Acc_BetList_AgentDetail", parameters);
        }

        public DataTable Acc_BetList_TotalBet_AgentDetail(long transId, long agentId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("transid", transId);
            parameters.Add("agentid", agentId);
            return ExecSpTable("Acc_BetList_TotalBet_AgentDetail", parameters);
        }

        #endregion
    }
}