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
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region css

            bundles.AddAndInclude(
                "~/css/new-message",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                "~/views/message/resources/message.less");

            bundles.AddAndInclude(
                "~/css/totalbets",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
                "~/views/totalbets/resources/totalbets.less",
                "~/_globalresources/css/refreshcountdown.less"
                );

            bundles.AddAndInclude(
                "~/css/reports",
                "~/_globalresources/css/jquery-ui-1.10.2/apollo/jquery-ui.css",
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
         "~/css/error",
          "~/views/error/resources/Index.less");

            #endregion css

            #region js

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
                "~/_globalresources/js/jquery.cookie.js",
                "~/_globalresources/js/blockui/jquery.blockui.js",
                "~/_globalresources/js/core.js"));

            bundles.Add(new ScriptBundle("~/scripts/resultjs").Include(
               "~/_globalresources/js/head.js",
               "~/_globalresources/js/jquery.js",
               "~/_globalresources/js/jquery.cookie.js",
               "~/_globalresources/js/blockui/jquery.blockui.js",
               "~/_globalresources/js/core.js",
               "~/_globalresources/js/pager/jquery.sbpager.js",
               "~/views/results/resources/result.js"
               ));

            #region Customer Setting

            bundles.Add(new ScriptBundle("~/scripts/editsettingjs").Include(
                "~/views/account/resources/editsetting.js"));

            bundles.Add(new ScriptBundle("~/scripts/editpositiontakingjs").Include(
                "~/views/account/resources/editpositiontaking.js"));

            bundles.Add(new ScriptBundle("~/scripts/custsettingjs").Include(
                "~/_globalresources/js/head.min.js",
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
                "~/views/account/resources/custsettingengine/commissionsetting.js"));

            #endregion Customer Setting

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
                "~/views/account/resources/menu.js",
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
                "~/_globalresources/js/refreshcountdown.js"
            ));

            #endregion js

            bundles.FileSetOrderList.Clear();
        }
    }

    internal static class BundleExtensions
    {
        public static void AddAndInclude(
            this BundleCollection bundles,
            string bundleVirtualPath, params string[] virtualPaths)
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