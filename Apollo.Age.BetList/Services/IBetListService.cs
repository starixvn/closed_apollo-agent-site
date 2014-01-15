// --------------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.17">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.05.20">Add more function get agent upline of member</change>
// </history>
namespace Apollo.Age.BetList.Services
{
    using System.Collections.Generic;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.Entities;
    using Apollo.BetList.Core.Entities;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Service Interface.
    /// </summary>
    public interface IBetListService
    {
        IList<ITicket> GetDetailAgentComm(BetListCriteria criteria);

        IList<ITicket> GetDetailWinLossAgentComm(BetListCriteria criteria);

        IList<ITicket> GetMixParlayDetailTicket(BetListCriteria criteria);

        TicketResult WagersTicket(WagersCriteria criteria);

        TicketResult GetTicketResult(BetListCriteria criteria);

        TicketResult GetTicketResult(BetListCriteria criteria, bool isIncludeOldData);

        TicketResult GetRunningByBettype(BetListCriteria criteria);

        TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria);

        TicketResult GetWinLossByDateAndMem(BetListCriteria criteria);

        SystemParlayData GetCombinationMixParlayDetail(SystemParlayDetailCriteria criteria);

        void GetUserIdByUserName(int loggedId, bool isAgent, string username, out int agentId, out int custId, out int errorCode);

        IEnumerable<Agent> GetAgentUpLineOfMember(int parentId, int custId);

        HSSFWorkbook ExportExcel(List<TicketText> ticketResult, string title);
    }
}