/*
 * Created 20130315@Lewis
 * Revision:
 * 
 */

namespace Sunplus.Agent.Model.BL.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sunplus.Agent.Model.BL.Base;
    using Sunplus.Agent.Model.DL.Reports;
    using System.Data;
    using Sunplus.Agent.Model.DL.Admin;

    public class AdminBO : BOBase
    {
        private readonly AdminDAO _adminDAO;

        public AdminBO(string netSessionId)
            : base(netSessionId)
        {
            _adminDAO = new AdminDAO();
        }

        public DataTable GetAllExchangeRates()
        {
            return _adminDAO.GetAllExchangeRates();
        }
    }
}
