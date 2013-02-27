/*
 * Created 20100827@Lee - Casino API logger class.
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.Casino
{
    using Sunplus.Agent.Model.DL.Casino;
    using Sunplus.Libraries.APILogger;

    public class CasinoAPILogger : IAPILogger
    {
        private byte asSystemId = 2;
        private GetCustId asCustId;
        private GetUserId asUserId;

        public CasinoAPILogger(byte systemId, GetUserId userId, GetCustId custId)
        {
            this.asSystemId = systemId;
            this.asUserId = userId;
            this.asCustId = custId;
        }

        public delegate int GetUserId();

        public delegate int GetCustId();

        #region IAPILogger Members

        public void Write(int actionId, string functionName, string trackingValue, string returnValue, int returnCode)
        {
            CasinoDAO dao = new CasinoDAO();
            int userId = this.asUserId();
            int custId = this.asCustId();
            dao.WriteLog_UserLog_CasinoAPI(this.asSystemId, userId, custId, actionId, functionName, trackingValue, returnValue, returnCode);
        }

        #endregion
    }
}
