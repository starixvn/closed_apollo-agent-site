// --------------------------------------------------------------------------
// <copyright file="BetListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.04.17">Implement function get detail agentComm for Popup</change>
// </history>

namespace Apollo.Age.BetList.Services
{
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;
    using Apollo.Age.BetList.Repositories.Impl;
    using Apollo.Age.BetList.Repositories.Interface;
    using Apollo.Age.Services.Impl;

    /// <summary>
    /// BetList Service
    /// </summary>
    public class BetListService : QueryService, IBetListService
    {
        private IBetListRepository _betListRepository { get; set; }

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
        /// <param name="betListRepository">BetList Repository</param>
        public BetListService(IBetListRepository betListRepository)
        {
            this._betListRepository = this._betListRepository ?? new BetListRepository();
        }

        public TicketResult GetDetailAgentComm(BetListCriteria criteria)
        {
            return this._betListRepository.GetDetailAgentComm(criteria);
        }
    }
}