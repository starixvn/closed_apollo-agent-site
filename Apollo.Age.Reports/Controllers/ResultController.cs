// --------------------------------------------------------------------------
// <copyright file="ResultController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Web;
    using System.Web.Mvc;
    using Apollo.Age.Reports.Services;

    public class ResultController : BaseController
    {
        private readonly ReportService _reportService;

        public ResultController() : this(null) { }

        public ResultController(ReportService reportService = null)
        {
            _reportService = reportService ?? new ReportService();
        }

        public ActionResult Index()
        {
            var balanceUpDay = _reportService.GetBalanceUpDay();

            return View(balanceUpDay);
        }
    }
}
