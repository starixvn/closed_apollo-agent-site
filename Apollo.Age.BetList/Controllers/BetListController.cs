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
    using System.Web.Mvc;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Web;

    /// <summary>
    /// BetList Controller
    /// </summary>
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

        public ViewResult Index(string type, int betType = (int)BetTypes.Handicap)
        {
            return this.View();
        }
    }
}