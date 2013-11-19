// --------------------------------------------------------------------------
// <copyright file="OutrightController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.2">Created</change>
// </history>

namespace Apollo.Age.Wagers.Controllers
{
    using System.Web.Mvc;
    using Apollo.Age.Core;
    using Apollo.Age.Wagers.Criterias.Outright;
    using Apollo.Age.Wagers.ViewModels.Outright;
    using Apollo.Age.Web.Web.Attribute;

    /// <summary>
    /// Outright Controller.
    /// </summary>        
    public class OutrightController : WagerController
    {
        /// <summary>
        /// Wager Outright.
        /// </summary>
        /// <param name="criteria">OutrightCriteria model.</param>
        /// <returns>View result.</returns>
        public ActionResult WagerOutright(OutrightCriteria criteria)
        {
            bool noPermission = PermissionChecking(false, true);

            if (noPermission)
            {
                return View("~/Views/Error/AccessDenied.cshtml");
            }

            criteria.LoggedId = criteria.LoggedId > 0 ? criteria.LoggedId : CurrentSession.AgentId;
            criteria.Sporttypes = SportId(criteria.SportName);
            criteria.IsSuperView = CurrentSession.IsSuper;

            var result = WagerService.GetWagerData<OutrightViewModel>(criteria);

            ViewBag.TotalItems = criteria.TotalItems;
            return View(result);
        }
    }
}