// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Web
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Resources;
    using System.Web.Mvc;
    using Apollo.Age.Resources;

    /// <summary>
    /// Home controller class.
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Index()
        {
            if (SessionManager.CurrentSession != null)
            {
                return View(SessionManager.CurrentSession);
            }

            return View();
        }

        /// <summary>
        /// This action is for testing ONLY.
        /// </summary>
        /// <returns>Action result.</returns>
        [NonAction]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
        public ActionResult Index2()
        {
            try
            {
                var usCulture = new CultureInfo("en-US"); // American English
                var twCulture = new CultureInfo("zh-TW"); // Chinese Traditional

                ResourceSet resourceSetEnUs = Lang.ResourceManager.GetResourceSet(usCulture, true, true);
                ResourceSet anotherResourceSet = Lang.ResourceManager.GetResourceSet(twCulture, true, true);

                var notTransResources = new Dictionary<string, string>();

                foreach (DictionaryEntry entry in resourceSetEnUs)
                {
                    string key = entry.Key.ToString();

                    var otherValue = anotherResourceSet.GetString(key);

                    if (!string.IsNullOrWhiteSpace(otherValue))
                    {
                        notTransResources.Add(key, otherValue);
                    }
                }

                return View(notTransResources);
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Pagers the plugin.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult PagerPlugin()
        {
            return View();
        }
    }
}