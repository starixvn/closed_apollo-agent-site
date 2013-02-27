namespace Sunplus.Agent.Model.DL.BetSummary
{
    using System;
    using System.Collections;
    using System.Data;

    public class BetSummaryDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        /// <summary>
        /// Get bet summary
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public DataTable Age_Statement_Mem_BetSummary(int custId, DateTime dateFrom, DateTime dateTo, string language)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);

            return ExecSpTable("Age_Statement_Mem_BetSummary", parameters);
        }       
    }
}
