// --------------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.04.17">Implement function get detail agentComm for Popup</change>
//   <change who="Kaden.Trinh" date="2013.05.20">Add more function get agent upline of member</change>
// </history>

namespace Apollo.Age.BetList.Services
{
    using System.Collections.Generic;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Repositories.Impl;
    using Apollo.Age.BetList.Repositories.Interface;
    using Apollo.Age.Entities;
    using Apollo.Age.Services.Impl;
    using Apollo.BetList.Core.Entities;
    using NPOI.HSSF.UserModel;
    using Apollo.Age.Web.Services;
    using NPOI.SS.UserModel;
    using NPOI.HSSF.Util;

    /// <summary>
    /// Bet List Service Class.
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        private IBetListRepository _betListRepository;
        private ExcelService _excelService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        public BetListService()
            : this(null)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="BetListService" /> class.
        /// </summary>
        /// <param name="betListRepository">Bet List Repository.</param>
        public BetListService(IBetListRepository betListRepository)
        {
            this._betListRepository = this._betListRepository ?? new BetListRepository();
            this._excelService = new ExcelService();
        }

        /// <summary>
        /// Get detail agent PT and agent commission.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>List of Ticket.</returns>
        public IList<ITicket> GetDetailAgentComm(BetListCriteria criteria)
        {
            return this._betListRepository.GetDetailAgentComm(criteria);
        }

        /// <summary>
        /// Get detail win loss agent PT and agent commission.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>List of Ticket.</returns>
        public IList<ITicket> GetDetailWinLossAgentComm(BetListCriteria criteria)
        {
            return this._betListRepository.GetDetailWinLossAgentComm(criteria);
        }

        /// <summary>
        /// Get detail ticket mix-parlay.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Ticket data.</returns>
        public IList<ITicket> GetMixParlayDetailTicket(BetListCriteria criteria)
        {
            return this._betListRepository.GetMixParlayDetailTicket(criteria);
        }

        /// <summary>
        /// Get bet list running on Total Bets menu.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Ticket result.</returns>
        public TicketResult GetRunningByBettype(BetListCriteria criteria)
        {
            return this._betListRepository.GetRunningByBettype(criteria);
        }

        /// <summary>
        /// Get all bets by date and member from Win loss listOutstanding.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Ticket result.</returns>
        public TicketResult GetWinLossByDateAndMem(BetListCriteria criteria)
        {
            return this._betListRepository.GetWinLossByDateAndMem(criteria);
        }

        /// <summary>
        /// Get bet list by match Id.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Ticket data.</returns>
        public TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria)
        {
            return this._betListRepository.GetRunningByBettypeAndMatch(criteria);
        }

        /// <summary>
        /// Get Combination Mix Parlay Detail.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>SystemParlay data.</returns>
        public SystemParlayData GetCombinationMixParlayDetail(SystemParlayDetailCriteria criteria)
        {
            return this._betListRepository.GetCombinationMixParlayDetail(criteria);
        }

        /// <summary>
        /// Get UserId By Username.
        /// </summary>
        /// <param name="loggedId">Logged id.</param>
        /// <param name="isAgent">Agent choice.</param>
        /// <param name="username">User name.</param>
        /// <param name="agentId">Output agent id.</param>
        /// <param name="custId">Output customer id.</param>
        /// <param name="errorCode">Output error code.</param>
        public void GetUserIdByUserName(int loggedId, bool isAgent, string username, out int agentId, out int custId, out int errorCode)
        {
            this._betListRepository.GetUserIdByUserName(loggedId, isAgent, username, out agentId, out custId, out errorCode);
        }

        /// <summary>
        /// Get Agent up line of member.
        /// </summary>
        /// <param name="parentId">Logged id.</param>
        /// <param name="custId">Customer id.</param>
        /// <returns>List of agent.</returns>
        public IEnumerable<Agent> GetAgentUpLineOfMember(int parentId, int custId)
        {
            return this._betListRepository.GetAgentUpLineOfMember(parentId, custId);
        }

        /// <summary>
        /// Get data bet list on Wagers page.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Ticket result.</returns>
        public TicketResult WagersTicket(WagersCriteria criteria)
        {
            criteria.UGroup = RevertCommGroupName(criteria.UGroup);
            return this._betListRepository.WagersTicket(criteria);
        }

        /// <summary>
        /// Gets the ticket result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Ticket result.</returns>
        public TicketResult GetTicketResult(BetListCriteria criteria)
        {
            return this._betListRepository.GetTicketResult(criteria);
        }

        /// <summary>
        /// Gets the ticket result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="isIncludeOldData">If set to <c>true</c> [is include old data].</param>
        /// <returns>Ticket result.</returns>
        public TicketResult GetTicketResult(BetListCriteria criteria, bool isIncludeOldData = false)
        {
            return this._betListRepository.GetTicketResult(criteria, isIncludeOldData);
        }


        public HSSFWorkbook ExportExcel(TicketResult ticketResult, string title)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();

            var sheet = hssfworkbook.CreateSheet(Resources.Lang.lblBetLists);

            _excelService.CustomizeColor(hssfworkbook);

            var fontHeaderWhite = _excelService.CreateFont(hssfworkbook, HSSFColor.WHITE.index, 10);
            var fontHeaderBlack = _excelService.CreateFont(hssfworkbook, HSSFColor.BLACK.index, 10);

            var headerStyleShare = _excelService.CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);
            var headerStyleMember = _excelService.CreateStyle(hssfworkbook, HSSFColor.GREY_25_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);
            var headerStyleDownline = _excelService.CreateStyle(hssfworkbook, HSSFColor.GREEN.index, HorizontalAlignment.CENTER, fontHeaderWhite);
            var headerStyleAgent = _excelService.CreateStyle(hssfworkbook, HSSFColor.GREY_50_PERCENT.index, HorizontalAlignment.CENTER, fontHeaderBlack);

            var bodyStyleRight = _excelService.CreateStyle(hssfworkbook, align: HorizontalAlignment.RIGHT);
            var bodyStyleLeft = _excelService.CreateStyle(hssfworkbook);

            IDataFormat format = hssfworkbook.CreateDataFormat();
            var numberFormat = format.GetFormat("#,##0.00");

            int rowIndex, startIndex;
            rowIndex = startIndex = 4;
            var columIndex = 0;
            var row = sheet.CreateRow(rowIndex);


            _excelService.CreateStringCell(row, columIndex++, headerStyleShare, Resources.BetList.time);
            _excelService.CreateStringCell(row, columIndex++, headerStyleShare, Resources.BetList.discount);
            _excelService.CreateStringCell(row, columIndex++, headerStyleMember, Resources.BetList.gameType);
            _excelService.CreateStringCell(row, columIndex++, headerStyleMember, Resources.BetList.explain);
            _excelService.CreateStringCell(row, columIndex++, headerStyleMember, Resources.BetList.amount);
            _excelService.CreateStringCell(row, columIndex++, headerStyleDownline, Resources.BetList.result);
            _excelService.CreateStringCell(row, columIndex++, headerStyleDownline, Resources.BetList.status);
            _excelService.CreateStringCell(row, columIndex++, headerStyleAgent, Resources.Lang.lblAgentShare );
            _excelService.CreateStringCell(row, columIndex++, headerStyleAgent, Resources.Lang.AgentCommission);

            rowIndex++;
            columIndex = 0;
            row = sheet.CreateRow(rowIndex);

            foreach (var item in ticketResult.TicketList)
            {
                columIndex = 0;
                row = sheet.CreateRow(rowIndex);
                _excelService.CreateStringCell(row, columIndex++, bodyStyleLeft, item.ShowTime);
            }

            return hssfworkbook;
        }
    }
}