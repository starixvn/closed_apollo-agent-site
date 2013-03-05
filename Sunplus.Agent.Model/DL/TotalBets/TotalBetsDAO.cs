/*
 * Created 20100506@Jacob - Data access object of TotalBet function
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.Model.DL.TotalBets
{
    using System.Collections;
    using System.Data;

    public class TotalBetsDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        #region TotalGoal

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_6(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            return ExecSpTable("Age_TotalBets_6", parameters);
        }

        #endregion TotalGoal

        #region HDPOU

        /// <summary>
        /// Get handicap over under for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_1_3_7_8(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            return ExecSpTable("Age_TotalBets_1_3_7_8", parameters);
        }

        #endregion HDPOU

        #region MoneyLine

        /// <summary>
        /// Get money line for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_20_21(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            return ExecSpTable("Age_TotalBets_20_21", parameters);
        }

        #endregion MoneyLine

        #region OddEvent_OneXTwo

        /// <summary>
        /// Get odd event and one x two for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_2_5_12_15_123(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            return ExecSpTable("Age_TotalBets_2_5_12_15_123", parameters);
        }

        #endregion OddEvent_OneXTwo

        #region OutRight

        /// <summary>
        /// Get outright for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_10(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            return ExecSpTable("Age_TotalBets_10", parameters);
        }

        #endregion OutRight

        #region HTFT

        /// <summary>
        /// Get HTFT for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_16(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            return ExecSpTable("Age_TotalBets_16", parameters);
        }

        #endregion HTFT

        #region FGLG

        /// <summary>
        /// Get FGLG for Agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_14(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            return ExecSpTable("Age_TotalBets_14", parameters);
        }

        #endregion FGLG

        #region Home/Draw/AwayNoBet

        /// <summary>
        /// Get Home/Draw/AwayNoBet for Agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_25_121_122(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            return ExecSpTable("Age_TotalBets_25_121_122", parameters);
        }

        #endregion Home/Draw/AwayNoBet

        #region NextGoal

        /// <summary>
        /// Get NextGoal for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel22_Ag(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel22_Ag", parameters);
        }

        /// <summary>
        /// Get NextGoal for master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel22_MAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_Sel22_MAg", parameters);
        }

        /// <summary>
        /// Get NextGoal for super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel22_SAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_Sel22_SAg", parameters);
        }

        #endregion NextGoal

        #region HorseRacing

        /// <summary>
        /// Get HorseRacing for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_SelHR_Ag(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_SelHR_Ag", parameters);
        }

        /// <summary>
        /// Get HorseRacing for master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_SelHR_MAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_SelHR_MAg", parameters);
        }

        /// <summary>
        /// Get HorseRacing for super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_SelHR_SAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_SelHR_SAg", parameters);
        }

        #endregion HorseRacing

        #region Financial

        /// <summary>
        /// Get Financial for agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel201_Ag(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel201_Ag", parameters);
        }

        /// <summary>
        /// Get Financial for master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel201_MAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_Sel201_MAg", parameters);
        }

        /// <summary>
        /// Get Financial for super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel201_SAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_Sel201_SAg", parameters);
        }

        #endregion Financial

        #region NumberGame

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_161(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);
            return ExecSpTable("Age_TotalBets_161", parameters);
        }

        #endregion NumberGame

        #region LiveCasino

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel162_Ag(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel162_Ag", parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel162_MAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel162_MAg", parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel162_SAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_Sel162_SAg", parameters);
        }

        #endregion LiveCasino

        #region VirtualSports

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel180_Ag(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel180_Ag", parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel180_MAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            return ExecSpTable("Age_TotalBets_Sel180_MAg", parameters);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_Sel180_SAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_Sel180_SAg", parameters);
        }

        #endregion VirtualSports

        #region GetOtherBettypes

        /// <summary>
        /// Get Other Bettypes for Agent
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_SelOthers_Ag(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_SelOthers_Ag", parameters);
        }

        /// <summary>
        /// Get Other Bettypes for Master
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_SelOthers_MAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_SelOthers_MAg", parameters);
        }

        /// <summary>
        /// Get Other Bettypes for Super
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_TotalBets_SelOthers_SAg(int custId, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("language", language);

            return ExecSpTable("Age_TotalBets_SelOthers_SAg", parameters);
        }

        #endregion GetOtherBettypes
    }
}