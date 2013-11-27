// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>

namespace Apollo.Age.WebApp
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle config.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public static void RegisterBundles(BundleCollection bundles)
        {
            // css
            bundles.AddAndInclude(
                "~/css/new-message",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                "~/views/message/resources/message.less");

            bundles.AddAndInclude(
                "~/css/totalbets",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                "~/views/totalbets/resources/totalbets.less",
                "~/_globalresources/css/refreshcountdown.less");

            bundles.AddAndInclude(
                "~/css/reports",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                "~/_globalresources/js/fanex.grid/fanex.grid.less",
                "~/views/reports/resources/reports.less");

            bundles.AddAndInclude(
                "~/css/results/match",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                "~/_globalresources/Js/Pager/jquery.sbpager.less",
                "~/views/reports/resources/reports.less",
                "~/views/results/resources/results.less");

            bundles.AddAndInclude(
                "~/css/forecast",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                "~/_globalresources/css/refreshcountdown.less",
                "~/_globalresources/js/errormsg/errormsg.less");

            bundles.AddAndInclude(
                "~/css/wagers",
                 "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                 "~/areas/wagers/styles/wagers.less",
                 "~/_globalresources/css/refreshcountdown.less",
                 "~/_globalresources/js/pager/jquery.sbpager.less");

            bundles.AddAndInclude(
             "~/css/betlist",
              "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
              "~/views/betlist/resources/betList.less");

            bundles.AddAndInclude(
           "~/css/viewlog",
            "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
            "~/views/log/resources/monPickr-0.0.3.css");

            bundles.AddAndInclude(
             "~/css/account",
              "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
              "~/views/account/resources/treeview.css",
              "~/views/account/resources/account.less",
              "~/views/account/resources/customerlist.less",
              "~/views/account/resources/customerSetting.less");

            bundles.AddAndInclude(
            "~/css/viewcustsetting",
            "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
            "~/views/account/resources/customerSetting.less");

            bundles.AddAndInclude(
             "~/css/error",
              "~/views/error/resources/Index.less");

            bundles.AddAndInclude(
             "~/css/custsettinglayout",
              "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
              "~/views/account/resources/custSettingengine/core.less",
              "~/views/account/resources/custSettingengine/coreoverride.less");

            bundles.AddAndInclude(
             "~/css/index",
              "~/views/home/resources/index.less");

            bundles.AddAndInclude(
             "~/css/log-login",
              "~/views/account/resources/loglogin.less");

            bundles.AddAndInclude(
           "~/css/editsettinglayout",
            "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
            "~/views/account/resources/custSettingengine/core.less",
            "~/views/account/resources/custSettingengine/coreoverride.less",
            "~/views/account/resources/customersetting.less");

            bundles.AddAndInclude(
             "~/css/sbpager",
              "~/_GlobalResources/Js/Pager/jquery.sbpager.less");

            bundles.AddAndInclude(
                "~/css/fundout",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                "~/_globalresources/css/agent.less");

            bundles.AddAndInclude(
           "~/css/assignfunction",
            "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
           "~/Views/PermissionUserControl/Resources/permissionusercontrol.less");

            // js
            bundles.Add(new ScriptBundle("~/scripts/topbarjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/header.js"));

            bundles.Add(new ScriptBundle("~/scripts/homejs").Include(
                    "~/_globalresources/js/head.js",
                    "~/_globalresources/js/jquery.js",
                    "~/_globalresources/js/core.js",
                    "~/_globalresources/js/jquery.cookie.js",
                    "~/views/home/resources/default.js"));

            bundles.Add(new ScriptBundle("~/scripts/reportjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/noty/jquery.noty.js",
                "~/_globalresources/js/core.js",
                "~/views/reports/resources/reports.js"));

            bundles.Add(new ScriptBundle("~/scripts/winlossjs").Include(
               "~/_globalresources/js/head.js",
               "~/_globalresources/js/jquery.js",
               "~/_globalresources/js/jquery-ui.js",
               "~/_globalresources/js/blockui/jquery.blockui.js",
               "~/_globalresources/js/core.js",
               "~/views/reports/resources/reports.js",
               "~/views/reports/resources/winloss.js"));

            bundles.Add(new ScriptBundle("~/scripts/sportcreditjs").Include(
               "~/_globalresources/js/head.js",
               "~/_globalresources/js/jquery.js",
               "~/_globalresources/js/jquery-ui.js",
               "~/_globalresources/js/blockui/jquery.blockui.js",
               "~/_globalresources/js/core.js",
               "~/views/reports/resources/reports.js",
               "~/views/reports/resources/sportcredit.js"));

            bundles.Add(new ScriptBundle("~/scripts/outstandingjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/_globalresources/js/fanex.grid/moment.js",
                "~/_globalresources/js/fanex.grid/numeral.js",
                "~/_globalresources/js/fanex.grid/fanex.grid.js",
                "~/views/reports/resources/reports.js",
                "~/views/reports/resources/outstanding.js"));

            bundles.Add(new ScriptBundle("~/scripts/totalbetsjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/jquery.cookie.js",
                "~/_globalresources/js/refreshcountdown.js",
                "~/_globalresources/js/core.js",
                "~/views/totalbets/resources/totalbets.js"));

            bundles.Add(new ScriptBundle("~/scripts/forecast-indexjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery.cookie.js",
                "~/_globalresources/js/refreshcountdown.js",
                "~/views/totalbets/resources/index.js"));

            bundles.Add(new ScriptBundle("~/scripts/forecastjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/jquery.cookie.js",
                "~/_globalresources/js/refreshcountdown.js",
                "~/_globalresources/js/errormsg/errormsg.js",
                "~/_globalresources/js/core.js"));

            bundles.Add(new ScriptBundle("~/scripts/messagejs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",                
                "~/_globalresources/js/core.js",
                "~/views/message/resources/portalpage.js"));

            bundles.Add(new ScriptBundle("~/scripts/logjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/log/resources/monpickr-0.0.3.js",
                "~/views/log/resources/log.js"));

            bundles.Add(new ScriptBundle("~/scripts/viewaccountdetails").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/viewaccountdetails.js"));

            bundles.Add(new ScriptBundle("~/scripts/accountjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/jquery.cookie.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/menu.js",
                "~/views/account/resources/accountinfo.js"));

            bundles.Add(new ScriptBundle("~/scripts/resultjs").Include(
               "~/_globalresources/js/head.js",
               "~/_globalresources/js/jquery.js",
               "~/_globalresources/js/jquery.cookie.js",
               "~/_globalresources/js/blockui/jquery.blockui.js",
               "~/_globalresources/js/core.js",
               "~/_globalresources/js/pager/jquery.sbpager.js",
               "~/views/results/resources/result.js"));

            // Customer Setting
            bundles.Add(new ScriptBundle("~/scripts/editsettingjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/noty/jquery.noty.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/custsettingengine/core.js",
                "~/views/account/resources/custsettingengine/ptsetting.js",
                "~/views/account/resources/custsettingengine/bettinglimit.js",
                "~/views/account/resources/custsettingengine/bettype.js",
                "~/views/account/resources/custsettingengine/bettypegroup.js",
                "~/views/account/resources/custsettingengine/commissionsetting.js",
                "~/views/account/resources/editsetting.js"));

            bundles.Add(new ScriptBundle("~/scripts/editpositiontakingjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/noty/jquery.noty.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/custsettingengine/core.js",
                "~/views/account/resources/custsettingengine/ptsetting.js",
                "~/views/account/resources/custsettingengine/bettinglimit.js",
                "~/views/account/resources/custsettingengine/bettype.js",
                "~/views/account/resources/custsettingengine/bettypegroup.js",
                "~/views/account/resources/custsettingengine/commissionsetting.js",
                "~/views/account/resources/editpositiontaking.js"));

            bundles.Add(new ScriptBundle("~/scripts/accountlistjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/accountlist.js"));

            bundles.Add(new ScriptBundle("~/scripts/logloginjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery.cookie.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js"));

            bundles.Add(new ScriptBundle("~/scripts/agenttreeviewjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery.cookie.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/menu.js",
                "~/views/account/resources/downlineinfo.js",
                "~/views/account/resources/jquery.treeview.js",
                "~/views/account/resources/jquery.treeview.async.js"));

            bundles.Add(new ScriptBundle("~/scripts/treeviewjs").Include(
                "~/views/account/resources/jquery.treeview.js",
                "~/views/account/resources/jquery.treeview.async.js"));

            bundles.Add(new ScriptBundle("~/scripts/viewsharesettingjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/viewshare.js"));

            bundles.Add(new ScriptBundle("~/scripts/viewcommsettingjs").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js",
                "~/views/account/resources/viewcomm.js"));

            bundles.Add(new ScriptBundle("~/scripts/unauthjs").Include(
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/core.js",
                "~/views/error/resources/unauthorized.js"));

            bundles.Add(new ScriptBundle("~/scripts/betlist").Include(
               "~/_globalresources/js/head.js",
               "~/_globalresources/js/jquery.js",
               "~/_globalresources/js/jquery-ui.js",
               "~/_globalresources/js/blockui/jquery.blockui.js",
               "~/_globalresources/js/jquery.cookie.js",
               "~/_globalresources/js/core.js",
               "~/views/betlist/resources/betlist.js"));

            bundles.Add(new ScriptBundle("~/scripts/wagers").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/Core.js",
                "~/_globalresources/js/BlockUI/jquery.blockUI.js",
                "~/areas/wagers/scripts/wagers.js",
                "~/_globalresources/js/pager/jquery.sbpager.js",
                "~/_globalresources/js/jquery.cookie.js",
                "~/_globalresources/js/refreshcountdown.js"));

            bundles.Add(new ScriptBundle("~/scripts/sbpager").Include(
            "~/_GlobalResources/Js/Pager/jquery.sbpager.js"));

            bundles.Add(new ScriptBundle("~/scripts/fundout").Include(
                "~/_globalresources/js/head.js",
                "~/_globalresources/js/jquery.js",
                "~/_globalresources/js/jquery-ui.js",
                "~/_globalresources/js/Core.js",
                "~/_globalresources/js/BlockUI/jquery.blockUI.js",
                 "~/views/account/resources/fundout.js"));

            bundles.Add(new ScriptBundle("~/scripts/assignfunction").Include(
            "~/_globalresources/js/jquery.js",
            "~/_globalresources/js/jquery-ui.js",
            "~/_globalresources/js/noty/jquery.noty.js",
            "~/_globalresources/js/Core.js",
            "~/_globalresources/js/BlockUI/jquery.blockUI.js",
            "~/Views/PermissionUserControl/Resources/AssignFunction.js"));

            bundles.FileSetOrderList.Clear();
        }
    }

    /// <summary>
    /// Bundle extensions.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    internal static class BundleExtensions
    {
        public static void AddAndInclude(
            this BundleCollection bundles,
            string bundleVirtualPath,
            params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new CssTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }

        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
}