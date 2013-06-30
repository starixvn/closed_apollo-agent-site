// --------------------------------------------------------------------------
// <copyright file="VolleyballBadmintonController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.6.25">Created</change>
//   <change who="Edward.Nguyen" date="2013.6.27">Implement prototype</change>
// </history>


namespace Apollo.Age.Wagers.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Apollo.Age.Wagers.ViewModels.Volleyball;
    using Apollo.Age.Wagers.Criterias.Volleyball;
    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VolleyballController : WagerController
    {
        public ActionResult Straight(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new VolleyballCriteria
            {

                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction
            };

            var result = _WagerService.GetWagerData<StraightViewModel>(criteria);

            return View(result);
        }


        public ActionResult Specialbet(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new VolleyballCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction
            };

            var result = _WagerService.GetWagerData<SpecialViewModel>(criteria);

            return View(result);
        }


        public ActionResult RunningBall(DateTime fromDate, string uGroup, bool isDirectMember, int leagueId)
        {
            fromDate = new DateTime(2013, 05, 23);
            var criteria = new VolleyballCriteria
            {
                FromDate = fromDate,
                IsDirectMem = isDirectMember,
                LeagueId = leagueId,
                LoggedId = CurrentSession.AgentId,
                UGroup = uGroup,
                GroupBetName = this.CurrentAction
            };

            var result = _WagerService.GetWagerData<RunningBallViewModel>(criteria);

            return View(result);
        }


        public ActionResult Parlay()
        {
            return View();
        }
    }
}
