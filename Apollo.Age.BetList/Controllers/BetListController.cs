// --------------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
// </history>

namespace Apollo.Age.BetList.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.Web;

    /// <summary>
    /// BetList Controller
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BetListController : BaseController
    {
        private readonly BetListService betListService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListController" /> class.
        /// </summary>
        public BetListController()
        {
            this.betListService = new BetListService();
        }

        public ViewResult Index()
        {
            return this.View();
        }

        public ViewResult ViewDetail(long transId, DateTime fromDate, DateTime toDate, string type)
        {
            return this.View();
        }
    }
}