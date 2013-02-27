/*
 * Created 20100116@Lee - IBC data access object of casino function
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.DL.Casino
{
    using System.Collections;

    public class CasinoDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        public void WriteLog_UserLog_CasinoAPI(byte systemId, int userId, int custId, int actionId, string funcName, string trackingValue, string returnValue, int returnCode)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("systemid", systemId);
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);
            parameters.Add("actionid", actionId);
            parameters.Add("funcname", funcName);
            parameters.Add("trackingvalue", trackingValue);
            parameters.Add("returnvalue", returnValue);
            parameters.Add("returncode", returnCode);

            ExecSp("WriteLog_UserLog_CasinoAPI", parameters);
        }
    }
}