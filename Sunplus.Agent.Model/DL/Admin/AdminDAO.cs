/*
 * Created 20130315@Lewis
 * Revision:
 * 
 */

namespace Sunplus.Agent.Model.DL.Admin
{
    using System.Collections;
    using System.Data;
    using Sunplus.Agent.Model.DL.Base;
    using Fanex.Data.DNAide;
    using System;

    public class AdminDAO : DAOBase
    {
        public DataTable GetAllExchangeRates()
        {
            return this.ExecSpTable("Age_Exchange_GetAll", new Hashtable());
        }

        public void UpdateExchangeRate(int custId, string ip,
            int exchangeId, string description, double multipliedBy,
            double baseRate, double minBet, double minBetLive,
            ref int errCode, ref string errMsg)
        {
            var parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("ip", ip);

            parameters.Add("exchangeId", exchangeId);
            parameters.Add("description", description);
            parameters.Add("multipliedBy", multipliedBy);
            parameters.Add("baseRate", baseRate);
            parameters.Add("minBet", minBet);
            parameters.Add("minBetLive", minBetLive);
            
            parameters.Add("errCode", 0);
            parameters.Add("errMsg", string.Empty);

            DbResult result = ExecSp("Age_Exchange_UpdateRate", parameters);

            errCode = Convert.ToInt32(result.Outputs["errCode"]);
            errMsg = result.Outputs["errMsg"].ToString();
        }
    }
}