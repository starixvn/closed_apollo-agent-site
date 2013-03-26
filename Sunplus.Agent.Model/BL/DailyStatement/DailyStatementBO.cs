namespace Sunplus.Agent.Model.BL.DailyStatement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.BL.Base;
    using Sunplus.Agent.Model.BL.Reports;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Base;

    public class DailyStatementBO : BOBase
    {
        public DailyStatementBO(string netSessionId)
            : base(netSessionId)
        { 
        }

        /// <summary>
        /// Get daily statement list
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public List<DailyStatementEntity> GetDailyStatementList(int custId, DateTime dateFrom, DateTime dateTo)
        {
            string language = CurrentSession.Language;

            DailyStatementDAO dao = new DailyStatementDAO();

            DataTable dataTable = dao.Age_Statement_Mem(custId, dateFrom, dateTo, language, ReportsBO.BalanceUpDate, ReportsBO.MoveBetsDate);

            IEnumerable<IEntity> enumerable = dataTable.AsEnumerable(typeof(DailyStatementEntity));
            List<DailyStatementEntity> dailyStatementList = new List<DailyStatementEntity>();

            foreach (IEntity entity in enumerable)
            {
                dailyStatementList.Add(entity as DailyStatementEntity);                
            }

            return dailyStatementList;
        }        
    }
}
