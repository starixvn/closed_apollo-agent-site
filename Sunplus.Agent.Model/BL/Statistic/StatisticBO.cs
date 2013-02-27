/*
 * Created 20100428@Lee - Statistic functions
 * Revision:
 * 20100428@Lee - Initializing
 * 
 * 
 * 
 */

namespace Sunplus.Agent.Model.BL.Statistic
{
    using Sunplus.Agent.Model.DL.Statistic;

    public class StatisticBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public StatisticBO(string netSessionId)
            : base(netSessionId)
        { 
        }

        public void UpdatePageVisit_Old(string url)
        {
            StatisticDAO dao = new StatisticDAO();
            dao.Age_UpdPageVisit(CurrentSession.RoleId, url);
        }

        public void UpdateFunctionUsage(string url, string function, string subFunction, string site)
        {
            StatisticDAO dao = new StatisticDAO();
            dao.IS_UpdFunctionUsage(url, CurrentSession.ServerId, function, subFunction, site, CurrentSession.LangKey);  
        }

        public void UpdateFunctionUsage(string function, string subFunction)
        {
            StatisticDAO dao = new StatisticDAO();
            dao.Agm_UpdFunctionUsage(CurrentSession.ServerId, function, subFunction, CurrentSession.LangKey);  
        }
    }
}
