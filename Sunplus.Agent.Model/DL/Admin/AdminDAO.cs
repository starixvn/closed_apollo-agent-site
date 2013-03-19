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

    public class AdminDAO : DAOBase
    {
        public DataTable GetAllExchangeRates()
        {
            return this.ExecSpTable("Age_Exchange_GetAll", new Hashtable());
        }
    }
}