namespace Sunplus.Agent.Model.DL.Forecast
{
    using System;
    using System.Collections;
    using System.Data;
    using Sunplus.Agent.Model.DL.Definitions;

    public class ForecastDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        private static string connection = DbTypes.ARCHIVE;

        public static string Connection
        {
            get { return ForecastDAO.connection; }
            set { ForecastDAO.connection = value; }
        }

        #region TopOddsByMatch

        /// <summary>
        /// Get the most odds of match
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="betType"></param>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_OddsSelTopByMatch(int matchId, int betType, string langKey)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            parameters.Add("language", langKey);
            return ExecSpTable("Age_Forecast_OddsSelTopByMatch", parameters);
        }

        #endregion TopOddsByMatch

        #region Handicap

        /// <summary>
        /// Forecast for handicap
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_SelHDP(int matchId, int custId, int roleId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("agentid", custId);
            parameters.Add("roleid", roleId);
            parameters.Add("bettype", betType);
            return ExecSpTable("Age_Forecast_SelHDP", parameters);
        }

        #endregion Handicap

        #region OverUnder

        /// <summary>
        /// Forecast for Over under
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_SelOU(int matchId, int custId, int roleId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("agentid", custId);
            parameters.Add("roleid", roleId);
            parameters.Add("bettype", betType);
            return ExecSpTable("Age_Forecast_SelOU", parameters);
        }

        #endregion OverUnder

        #region NextGoal

        /// <summary>
        /// Forecast for next goal
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_Sel22(int matchId, int custId, int roleId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("custid", custId);
            parameters.Add("roleid", roleId);
            return ExecSpTable("Age_Forecast_Sel22", parameters);
        }

        #endregion NextGoal

        #region Running League

        /// <summary>
        /// Get running league for agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_LeagueSelRunning_Ag(int agentId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agid", agentId);
            parameters.Add("bettypes", betType);
            return ExecSpTable("Age_LeagueSelRunning_Ag", parameters);
        }

        /// <summary>
        /// Get running leagues for master
        /// </summary>
        /// <param name="masterId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_LeagueSelRunning_MAg(int masterId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("magid", masterId);
            parameters.Add("bettypes", betType);
            return ExecSpTable("Age_LeagueSelRunning_MAg", parameters);
        }

        /// <summary>
        /// Get running leagues for super
        /// </summary>
        /// <param name="superId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_LeagueSelRunning_SAg(int superId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("sagid", superId);
            parameters.Add("bettypes", betType);
            return ExecSpTable("Age_LeagueSelRunning_SAg", parameters);
        }

        #endregion Running League

        #region League

        /// <summary>
        /// Forecast league for agent
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="agentId"></param>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_SelByLeague_Ag(int leagueId, int agentId, string langKey)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("language", langKey);
            parameters.Add("baseoffsethdp", 0);
            parameters.Add("baseoffsetou", 0);
            parameters.Add("baseoffseththdp", 0);
            parameters.Add("baseoffsethtou", 0);
            parameters.Add("leagueid", leagueId);
            parameters.Add("recommend", agentId);

            return ExecSpTable("Age_Forecast_SelByLeague_Ag", parameters);
        }

        /// <summary>
        /// Forecast league for master
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="masterId"></param>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_SelByLeague_MAg(int leagueId, int masterId, string langKey)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("language", langKey);
            parameters.Add("baseoffsethdp", 0);
            parameters.Add("baseoffsetou", 0);
            parameters.Add("baseoffseththdp", 0);
            parameters.Add("baseoffsethtou", 0);
            parameters.Add("leagueid", leagueId);
            parameters.Add("mrecommend", masterId);

            return ExecSpTable("Age_Forecast_SelByLeague_MAg", parameters);
        }

        /// <summary>
        /// Forecast league for super
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="superId"></param>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_SelByLeague_SAg(int leagueId, int superId, string langKey)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("language", langKey);
            parameters.Add("baseoffsethdp", 0);
            parameters.Add("baseoffsetou", 0);
            parameters.Add("baseoffseththdp", 0);
            parameters.Add("baseoffsethtou", 0);
            parameters.Add("leagueid", leagueId);
            parameters.Add("srecommend", superId);

            return ExecSpTable("Age_Forecast_SelByLeague_SAg", parameters);
        }

        #endregion League

        #region 1x2ByLeague

        /// <summary>
        /// Forecast 1X2 league for agent
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public DataTable Age_Forecast1x2_SelByLeague_Ag(int leagueId, int agentId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("leagueid", leagueId);
            parameters.Add("custid", agentId);
            return ExecSpTable("Age_Forecast1x2_SelByLeague_Ag", parameters);
        }

        /// <summary>
        /// Forecast 1X2 by league for master
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public DataTable Age_Forecast1x2_SelByLeague_MAg(int leagueId, int masterId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("leagueid", leagueId);
            parameters.Add("custid", masterId);
            return ExecSpTable("Age_Forecast1x2_SelByLeague_MAg", parameters);
        }

        /// <summary>
        /// Forecast 1X2 by league for super
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="superId"></param>
        /// <returns></returns>
        public DataTable Age_Forecast1x2_SelByLeague_SAg(int leagueId, int superId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("leagueid", leagueId);
            parameters.Add("custid", superId);
            return ExecSpTable("Age_Forecast1x2_SelByLeague_SAg", parameters);
        }

        #endregion 1x2ByLeague

        #region 1X2ByMatch

        /// <summary>
        /// Forecast 1X2 by match.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_Sel1X2(int matchId, int custId, int roleId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("custid", custId);
            parameters.Add("bettype", betType);
            parameters.Add("roleid", roleId);
            return ExecSpTable("Age_Forecast_Sel1X2", parameters);
        }

        #endregion 1X2ByMatch

        #region For Score Map

        /// <summary>
        /// Get Score Map League
        /// </summary>
        /// <returns></returns>
        public DataTable Age_ScoreMap_SelLeague(int agentId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", agentId);
            return ExecSpTable("Age_ScoreMap_SelLeague", parameters);
        }

        /// <summary>
        /// Get Match By League
        /// </summary>
        /// <returns></returns>
        public DataTable Age_ScoreMap_SelMatch(int agentId, int leagueId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", agentId);
            parameters.Add("leagueid", leagueId);
            return ExecSpTable("Age_ScoreMap_SelMatch", parameters);
        }

        /// <summary>
        /// Bet Type Score Map
        /// </summary>
        /// <returns></returns>
        public DataTable Age_ScoreMap_SelBettype()
        {
            return ExecSpTable("Age_ScoreMap_SelBettype", null);
        }

        /// <summary>
        /// Get Score Map Result for Agent
        /// </summary>
        /// <returns></returns>
        public DataSet Age_ScoreMap_Result(int customerId, string matchIds, string bettypes_live, string bettypes_dead)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", customerId);
            parameters.Add("matchids", matchIds);
            parameters.Add("bettypes_live", bettypes_live);
            parameters.Add("bettypes_dead", bettypes_dead);
            return ExecSpDataset("Age_ScoreMap_Result", parameters);
        }

        #endregion For Score Map
    }
}