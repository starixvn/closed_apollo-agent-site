/*
 * Created 20100428@Lee - Statistic functions
 * Revision:
 * 20100428@Lee - Initializing
 * 
 * 
 * 
 */
namespace Sunplus.Agent.Model.DL.Statistic
{
    using System.Collections;

    public class StatisticDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        public StatisticDAO()
        {
        }

        public void Age_UpdPageVisit(int roleId, string pageUrl)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("role", roleId);
            parameters.Add("pageurl", pageUrl);
            ExecSp("Age_UpdPageVisit", parameters);
        }

        public void IS_UpdFunctionUsage(string pageUrl, int serverId, string function, string subFunction, string site, string lang)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("pageurl", pageUrl);
            parameters.Add("serverid", serverId);
            parameters.Add("function", function);
            parameters.Add("subfunction", subFunction);
            parameters.Add("site", site);
            parameters.Add("lang", lang);
            ExecSp("IS_UpdFunctionUsage", parameters);  
        }

        public void Agm_UpdFunctionUsage(int serverId, string function, string subFunction, string lang)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("serverid", serverId);
            parameters.Add("function", function);
            parameters.Add("subfunction", subFunction);
            parameters.Add("lang", lang);
            ExecSp("Agm_UpdFunctionUsage", parameters);
        }
    }
}
