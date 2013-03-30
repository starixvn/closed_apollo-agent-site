// --------------------------------------------------------------------------
// <copyright file="TotalBetController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Web;
    using Apollo.Age.TotalBets.Services;
    using System.Web.Mvc;
    using Apollo.Age.TotalBets.ViewModels;
    using Apollo.Age.TotalBets.Criterias;

    public class TotalBetController : BaseController
    {
        private readonly TotalBetsServiceBase _totalBetsService;

        public TotalBetController()
        {
            _totalBetsService = new TotalBetsServiceBase();
        }

        public TotalBetsServiceBase TotalBetService
        {
            get
            {
                return _totalBetsService;
            }
        }

        private const string ViewPath = "~/Views/TotalBets/{0}.cshtml";
        protected ViewResult TotalBetsView(object model, string path = ViewPath)
        {
            return View(string.Format(ViewPath, ControllerName), model);
        }

        protected string ControllerName
        {
            get {
                return RouteData.Values["controller"].ToString();
            }
        }

        
    }
}
