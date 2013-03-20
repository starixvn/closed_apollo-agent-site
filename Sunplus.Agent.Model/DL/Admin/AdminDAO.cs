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
    using System.Collections.Generic;
    using Fanex.Age.DTO.Admin.ExchangeRate;

    public class AdminDAO : DAOBase
    {
        public ExchRate GetExchangeRateById(int exchangeId)
        {
            var parameters = new Hashtable();

            parameters.Add("exchangeid", exchangeId);

            var dt = ExecSpTable("Age_Exchange_GetById", parameters);

            if (dt.Rows.Count > 0)
            {
                var rate = dt.Rows[0];

                return new ExchRate
                {
                    ExchangeId = Convert.ToInt32(rate["ExchangeId"]),
                    Currency = rate["Currency"].ToString(),
                    MultipliedBy = Convert.ToInt32(rate["MultipliedBy"]),
                    BaseRate = Convert.ToDouble(rate["ActualRate"]),
                    MinBet = Convert.ToDouble(rate["MinBet"]),
                    MinBetLive = Convert.ToDouble(rate["MinBetLive"]),
                    Description = rate["Description"].ToString()
                };
            }

            return null;
        }

        public ExchRate GetExchangeRateByCurrency(string currency)
        {
            var parameters = new Hashtable();

            parameters.Add("currency", currency);

            var dt = ExecSpTable("Age_Exchange_GetByCurrency", parameters);

            if (dt.Rows.Count > 0)
            {
                var rate = dt.Rows[0];

                return new ExchRate
                {
                    ExchangeId = Convert.ToInt32(rate["ExchangeId"]),
                    Currency = rate["Currency"].ToString(),
                    MultipliedBy = Convert.ToInt32(rate["MultipliedBy"]),
                    BaseRate = Convert.ToDouble(rate["ActualRate"]),
                    MinBet = Convert.ToDouble(rate["MinBet"]),
                    MinBetLive = Convert.ToDouble(rate["MinBetLive"]),
                    Description = rate["Description"].ToString()
                };
            }

            return null;
        }

        public DataTable GetAllExchangeRates()
        {
            return this.ExecSpTable("Age_Exchange_GetAll", new Hashtable());
        }

        public void InsertExchangeRate(int custId, string ip,
            string currency, string description, double multipliedBy,
            double baseRate, double minBet, double minBetLive,
            out int exchangeId, ref int errCode, ref string errMsg)
        {
            var parameters = new Hashtable();

            parameters.Add("currentuserid", custId);
            parameters.Add("loginip", ip);
            parameters.Add("logdate", DateTime.Now);

            parameters.Add("exchangeid", 0);
            parameters.Add("currency", currency);
            parameters.Add("description", description);
            parameters.Add("multipliedby", multipliedBy);
            parameters.Add("baserate", baseRate);
            parameters.Add("minbet", minBet);
            parameters.Add("minbetlive", minBetLive);

            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);

            DbResult result = ExecSp("Age_Exchange_InsertRate", parameters);

            exchangeId = Convert.ToInt32(result.Outputs["exchangeid"]);
            errCode = Convert.ToInt32(result.Outputs["errcode"]);
            errMsg = result.Outputs["errmsg"].ToString();
        }

        public void UpdateExchangeRate(int custId, string ip,
            int exchangeId, string description, double multipliedBy,
            double baseRate, double minBet, double minBetLive,
            ref int errCode, ref string errMsg)
        {
            var parameters = new Hashtable();

            parameters.Add("currentuserid", custId);
            parameters.Add("loginip", ip);
            parameters.Add("logdate", DateTime.Now);

            parameters.Add("exchangeid", exchangeId);
            parameters.Add("description", description);
            parameters.Add("multipliedby", multipliedBy);
            parameters.Add("baserate", baseRate);
            parameters.Add("minbet", minBet);
            parameters.Add("minbetlive", minBetLive);

            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);

            DbResult result = ExecSp("Age_Exchange_UpdateRate", parameters);

            errCode = Convert.ToInt32(result.Outputs["errcode"]);
            errMsg = result.Outputs["errmsg"].ToString();
        }
    }
}