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
    using System.Globalization;
    using System.Resources;
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {       
            return View(SessionManager.CurrentSession);
        }
              
        /// <summary>
        /// This action is for testing ONLY.
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
                    Apollo.Age.Resources.Lang.ResourceManager.GetResourceSet(usCulture, true, true);
                ResourceSet anotherResourceSet =
                    Apollo.Age.Resources.Lang.ResourceManager.GetResourceSet(cnCulture, true, true);

                ResourceSet anotherResourceSet1 =
                    Apollo.Age.Resources.Lang.ResourceManager.GetResourceSet(twCulture, true, true);

                var notTransResources = new Dictionary<string, string>();
                var hasKeybutNotTransResources = new Dictionary<string, string>();

                foreach (DictionaryEntry entry in resourceSetEnUs)
                {
                    string key = entry.Key.ToString();
                    string value = entry.Value.ToString();

                    var otherValue = anotherResourceSet1.GetString(key);
                    //var otherValue1 = anotherResourceSet1.GetString(key);

                    if (!string.IsNullOrWhiteSpace(otherValue))
                    {
                        notTransResources.Add(key, otherValue);
                    }
                    //else
                    //{
                    //    if (value.Equals(otherValue1) && !value.Equals(otherValue))
                    //    {
                    //        hasKeybutNotTransResources.Add(key, value);
                    //    }
                    //}

                    //hasKeybutNotTransResources.Add(key, value);
                }

                return View(notTransResources);
            }
            catch 
            {
                return View();
            }
        }

        public ActionResult PagerPlugin()
        {
            return View();
        }
    }
}