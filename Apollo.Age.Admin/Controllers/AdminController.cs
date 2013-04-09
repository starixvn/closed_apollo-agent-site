// --------------------------------------------------------------------------
// <copyright file="AdminController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Web;
    using Sunplus.Agent.Model.Entities;
    using Apollo.Age.Admin.Services;
    using Apollo.Age.DTO.Admin.ExchangeRate;

    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;

        public AdminController()
            : this(null) { }

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService ?? new AdminService();
        }

        public ActionResult ExchangeRate()
        {
            var exchangeRateList = new List<ExchRate>();

            return View();
        }
    }
}