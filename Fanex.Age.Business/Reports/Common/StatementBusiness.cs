/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Business.Reports.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Common;

    public class StatementBusiness
    {
        private bool isAgentSite;

        public StatementBusiness(bool isAgentSite)
        {
            this.isAgentSite = isAgentSite;
        }

        public IList<StatementItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new StatementItem
                         {
                             Date = this.isAgentSite ? c.Field<DateTime>("accdate") : c.Field<DateTime>("matchdate"),
                             TransType = c.Field<string>("transtype"),
                             TransDesc = c.Field<string>("transdesc"),
                             TotalAmt = c.Field<decimal>("totalamt"),
                             RowNum = c.Field<long>("rownum")
                         }).ToList();

            return result;
        }
    }
}