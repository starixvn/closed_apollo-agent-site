/*
 * Created 20130315@Lewis
 * Revision:
 * 
 */

namespace Sunplus.Agent.Model.BL.Admin
{
    using System.Data;
    using Fanex.Age.DTO.Admin.ExchangeRate;
    using Sunplus.Agent.Model.BL.Base;
    using Sunplus.Agent.Model.DL.Admin;

    public class AdminBO : BOBase
    {
        private readonly AdminDAO _adminDAO;

        public AdminBO(string netSessionId)
            : base(netSessionId)
        {
            _adminDAO = new AdminDAO();
        }

        public ExchRate GetExchangeRateById(int exchangeId)
        {
            return _adminDAO.GetExchangeRateById(exchangeId);
        }

        public ExchRate GetExchangeRateByCurrency(string currency)
        {
            return _adminDAO.GetExchangeRateByCurrency(currency);
        }

        public DataTable GetAllExchangeRates()
        {
            return _adminDAO.GetAllExchangeRates();
        }

        public void InsertExchangeRate(int custId, string ip,
            string currency, string description, double multipliedBy,
            double baseRate, double minBet, double minBetLive,
            out int exchangeId, ref int errCode, ref string errMsg)
        {
            _adminDAO.InsertExchangeRate(custId, ip,
                currency, description, multipliedBy,
                baseRate, minBet, minBetLive,
                out exchangeId, ref errCode, ref errMsg);
        }

        public void UpdateExchangeRate(int custId, string ip,
            int exchangeId, string description, double multipliedBy,
            double baseRate, double minBet, double minBetLive,
            ref int errCode, ref string errMsg)
        {
            _adminDAO.UpdateExchangeRate(custId, ip,
                exchangeId, description, multipliedBy,
                baseRate, minBet, minBetLive,
                ref errCode, ref errMsg);
        }
    }
}
