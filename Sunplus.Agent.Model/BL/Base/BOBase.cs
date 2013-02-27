/*
 * Created 20100115@Lee - Business access base class
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.Base
{
    using Sunplus.Agent.Model.Entities;

    public class BOBase
    {
        private IUserSession asUserSession;
        private string asNetSessionId;

        public BOBase(string netSessionId)
        {
            this.asNetSessionId = netSessionId;
            this.asUserSession = UserSessionService.GetUserSessionInstance(netSessionId);
        }

        public IUserSession CurrentSession
        {
            get
            {
                return this.asUserSession;
            }
        }

        public string NetSessionId
        {
            get
            {
                return this.asNetSessionId;
            }
        }
    }
}
