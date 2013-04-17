// --------------------------------------------------------------------------
// <copyright file="BetListRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
// </history>

namespace Apollo.Age.BetList.Repositories.Impl
{
    using System.Data;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;
    using Apollo.Age.BetList.Repositories.Interface;
    using Fanex.BetList.Core.Entities;
    using Fanex.Data;

    public class BetListRepository : IBetListRepository
    {
        public TicketResult GetDetailAgentComm(BetListCriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    var dataSet = db.ExecuteDataSet(criteria.GetSpParams());
                    var ticketResult = new TicketResult();

                    // Main ticket list
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    ticketResult.TicketList = CollectionHelper.ConvertTo<ITicket>(mainTable);

                    // Reference ticket
                    var referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();
                    ticketResult.TicketDataList = CollectionHelper.ConvertTo<ITicketData>(referenceTable);
                    return ticketResult;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}