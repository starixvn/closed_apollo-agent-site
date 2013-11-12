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

    /// <summary>
    /// Instruction controller class.
    /// </summary>
    public class InstructionController : BaseController
    {
        /// <summary>
        /// Indexes the specified key.
        /// </summary>
        /// <param name="key">The key that need to authentication.</param>
        /// <param name="trigger">The trigger.</param>
        /// <param name="title">The title.</param>
        /// <param name="tipTitle">The tip title.</param>
        /// <returns>Action result.</returns>
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