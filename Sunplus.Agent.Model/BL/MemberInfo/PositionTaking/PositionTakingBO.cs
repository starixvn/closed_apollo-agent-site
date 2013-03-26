/*
 * Created 20100413@Lee - Business object of position taking function
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.MemberInfo.PositionTaking
{
    using System.Data;
    using Sunplus.Agent.Model.DL.MemberInfo.PositionTaking;
    using Sunplus.Agent.Model.BL.Base;

    public class PositionTakingBO : BOBase
    {
        public PositionTakingBO(string netSessionId)
            : base(netSessionId)
        { 
        }

        public DataTable GetMasterCollection(int custId)
        {
            PositionTakingDAO dao = new PositionTakingDAO();
            return dao.Age_CustomerSelMAgentList(custId);
        }

        public DataTable GetAgentCollection(int custId)
        {
            PositionTakingDAO dao = new PositionTakingDAO();
            return dao.Age_CustomerSelAgentList(custId);
        }
    }
}
