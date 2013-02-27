namespace Sunplus.Agent.Model.BL.BetSummary
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.DL.BetSummary;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Base;

    public class BetSummaryBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public BetSummaryBO(string netSessionId)
            : base(netSessionId)
        { 
        }

        /// <summary>
        /// Get Bet summary list
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public List<BetSummaryEntity> GetBetSummaryList(int custId, DateTime dateFrom, DateTime dateTo)
        {
            string language = CurrentSession.Language;

            BetSummaryDAO dao = new BetSummaryDAO();

            DataTable dataTable = dao.Age_Statement_Mem_BetSummary(custId, dateFrom, dateTo, language);

            IEnumerable<IEntity> enumerable = dataTable.AsEnumerable(typeof(BetSummaryEntity));
            List<BetSummaryEntity> betSummaryList = new List<BetSummaryEntity>();

            foreach (IEntity entity in enumerable)
            {
                betSummaryList.Add(entity as BetSummaryEntity);                
            }

            return betSummaryList;
        }
    }
}
