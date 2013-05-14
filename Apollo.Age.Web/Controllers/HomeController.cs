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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Resources;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Web.Services;
    using System.Net;

    public class HomeController : BaseController
    {
        private readonly UnderMaintenanceService maintenanceService;

        public HomeController()
        {
            maintenanceService = new UnderMaintenanceService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnderMaintenance()
        {
            var maintenanceInfo = maintenanceService.MaintenanceInfo;

            if (maintenanceInfo.IsDownTime)
            {
                return View(maintenanceInfo);
            }
            else
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        [AllowAnonymous]
        public ActionResult Error(string err)
        {
            switch (err)
            {
                case "403":
                    ViewBag.ErrorMessage = Apollo.Age.Resources.Lang.HttpError_403;
                    break;
                case "404":
                    ViewBag.ErrorMessage = Apollo.Age.Resources.Lang.HttpError_404;
                    break;
                default:
                    ViewBag.ErrorMessage = Apollo.Age.Resources.Lang.HttpError_Default;
                    break;
            }

            return View();
        }

        /// <summary>
        /// This action is for testing ONLY
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public ActionResult Index2()
        {
            try
            {
                var usCulture = new CultureInfo("en-US"); // American English
                var cnCulture = new CultureInfo("zh-CN"); // Chinese Simplified
                var twCulture = new CultureInfo("zh-TW"); // Chinese Traditional
                var viCulture = new CultureInfo("vi-VN"); // Vietnamese
                var jpCulture = new CultureInfo("ja-JP"); // Japanese
                var thCulture = new CultureInfo("th-TH"); // Thai
                var krCulture = new CultureInfo("ko-KR"); // Korean

                ResourceSet resourceSetEnUs =
                    Apollo.Age.Resources.Lang.ResourceManager.GetResourceSet(thCulture, true, true);
                ResourceSet anotherResourceSet =
                    Apollo.Age.Resources.Lang.ResourceManager.GetResourceSet(viCulture, true, true);
                var notTransResources = new Dictionary<string, string>();

                foreach (DictionaryEntry entry in resourceSetEnUs)
                {
                    string key = entry.Key.ToString();
                    string value = entry.Value.ToString();

                    if (string.IsNullOrWhiteSpace(anotherResourceSet.GetString(key)))
                    {
                        notTransResources.Add(key, value);
                    }
                }

                return View(notTransResources);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}