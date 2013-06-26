// --------------------------------------------------------------------------
// <copyright file="HiddenToolController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Alice" date="2013.6.25">Created</change>
// </history>

namespace Apollo.Age.HiddenTool.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Web;
    using System.Web.Mvc;

    public class HiddenToolController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NetworkEncryptionSetting()
        {
            return View();
        }

        public ActionResult BrowsingHistoryDeletion()
        {
            return View();
        }
        
    }
}