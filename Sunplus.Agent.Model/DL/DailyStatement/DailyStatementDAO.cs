namespace Sunplus.Agent.Model.BL.DailyStatement
{
    using System;
    using System.Collections;
    using System.Data;

    public class DailyStatementDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        /// <summary>
        /// Get daily statement outstanding
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="language"></param>
        /// <param name="balanceupday"></param>
        /// <param name="movebetday"></param>
        /// <returns></returns>
        public DataTable Age_Statement_Mem(int custId, DateTime dateFrom, DateTime dateTo, string language, DateTime balanceupday, DateTime movebetday)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("fromdate", dateFrom);
            parameters.Add("todate", dateTo);
            parameters.Add("language", language);
            parameters.Add("balanceupday", balanceupday);
            parameters.Add("movebetday", movebetday);

            return ExecSpTable("Age_Statement_Mem", parameters);
        }
    }
}
