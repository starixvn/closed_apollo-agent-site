// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.2">Created</change>
// </history>

namespace Apollo.Age.WebApp.Areas.Wagers
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/Wagers").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/Core.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/Areas/Wagers/Scripts/Wagers.js",
                "~/_GlobalResources/Js/Pager/jquery.sbpager.js",
                "~/_GlobalResources/Js/jquery.cookie.js",
                "~/_GlobalResources/Js/RefreshCountDown.js"
            ));

        }
    }
}