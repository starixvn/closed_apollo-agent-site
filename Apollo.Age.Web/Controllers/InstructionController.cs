// --------------------------------------------------------------------------
// <copyright file="InstructionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.6">Created</change>
// </history>

namespace Apollo.Age.Web
{
    using System.Web.Mvc;

    public class InstructionController : BaseController
    {
        public ActionResult Index(string key, string trigger = "", string title = "", string tipTitle = "")
        {
            ViewBag.Trigger = trigger;
            ViewBag.Title = title;
            ViewBag.TipTitle = tipTitle;

            string viewName = string.Format("~/Views/Instruction/{0}/{1}.cshtml", CurrentSession.Language, key);

            return View(viewName);
        }
    }
}