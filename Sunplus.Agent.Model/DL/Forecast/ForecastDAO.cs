namespace Sunplus.Agent.Model.DL.Forecast
{
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
        /// <returns></returns>
        public DataTable Age_Forecast_OddsGetTopByMatch(int matchId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("bettype", betType);
            return ExecSpTable("Age_Forecast_OddsGetTopByMatch", parameters);
        }

        #endregion TopOddsByMatch

        #region Handicap

        /// <summary>
        /// Forecast for handicap
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_1_7_GetByMatch(int matchId, int custId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("agentid", custId);
            parameters.Add("bettype", betType);
            return ExecSpTable("Age_Forecast_1_7_GetByMatch", parameters);
        }

        #endregion Handicap

        #region OverUnder

        /// <summary>
        /// Forecast for Over under
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="custId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_3_8_GetByMatch(int matchId, int custId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("agentid", custId);
            parameters.Add("bettype", betType);
            return ExecSpTable("Age_Forecast_3_8_GetByMatch", parameters);
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
        //public DataTable Age_Forecast_Sel22(int matchId, int custId, int roleId)
        //{
        //    Hashtable parameters = new Hashtable();
        //    parameters.Add("matchid", matchId);
        //    parameters.Add("custid", custId);
        //    parameters.Add("roleid", roleId);
        //    return ExecSpTable("Age_Forecast_Sel22", parameters);
        //}

        #endregion NextGoal

        #region Running League

        /// <summary>
        /// Get running league for agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="betType"></param>
        /// <returns></returns>
        public DataTable Age_League_GetRunning(int agentId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agid", agentId);
            parameters.Add("bettypes", betType);
            return ExecSpTable("Age_League_GetRunning", parameters);
        }


        #endregion Running League

        #region League

        /// <summary>
        /// Forecast league for agent
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_1_3_7_8_GetByLeague(int leagueId, int agentId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("baseoffsethdp", 0);
            parameters.Add("baseoffsetou", 0);
            parameters.Add("baseoffseththdp", 0);
            parameters.Add("baseoffsethtou", 0);
            parameters.Add("leagueid", leagueId);
            parameters.Add("AgentId", agentId);

            return ExecSpTable("Age_Forecast_1_3_7_8_GetByLeague", parameters);
        }


        #endregion League

        #region 1x2ByLeague

        /// <summary>
        /// Forecast 1X2 league for agent
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public DataTable Age_Forecast_5_15_GetByLeague(int leagueId, int agentId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("leagueid", leagueId);
            parameters.Add("AgentId", agentId);
            return ExecSpTable("Age_Forecast_5_15_GetByLeague", parameters);
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
        public DataTable Age_Forecast_5_15_GetByMatch(int matchId, int custId, int betType)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("matchid", matchId);
            parameters.Add("AgentId", custId);
            parameters.Add("bettype", betType);
            return ExecSpTable("Age_Forecast_5_15_GetByMatch", parameters);
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