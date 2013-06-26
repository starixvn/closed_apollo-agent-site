using System.Web;
using System.Web.Optimization;

namespace Apollo.Age.WebApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/homejs").Include(
            "~/_GlobalResources/Js/head.js",
            "~/_GlobalResources/Js/jquery.js",
            "~/_GlobalResources/Js/Core.js",
            "~/_GlobalResources/Js/jquery.cookie.js",
            "~/Views/Home/Resources/Default.js"));

            bundles.Add(new ScriptBundle("~/scripts/reportjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/noty/jquery.noty.js",
                "~/_GlobalResources/Js/Core.js",
                "~/Views/Reports/Resources/Reports.js"));

            bundles.Add(new ScriptBundle("~/scripts/outstandingjs").Include(
                "~/_GlobalResources/Js/Fanex.Grid/moment.js",
                "~/_GlobalResources/Js/Fanex.Grid/numeral.js",
                "~/_GlobalResources/Js/Fanex.Grid/Fanex.Grid.js"));

            bundles.Add(new ScriptBundle("~/scripts/totalbetsjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/jquery.cookie.js",
                "~/_GlobalResources/Js/RefreshCountDown.js",
                "~/_GlobalResources/Js/Core.js",
                "~/Views/TotalBets/Resources/TotalBets.js"));

            bundles.Add(new ScriptBundle("~/scripts/forecast-indexjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery.cookie.js",
                "~/_GlobalResources/Js/RefreshCountDown.js",
                "~/Views/TotalBets/Resources/Index.js"));

            bundles.Add(new ScriptBundle("~/scripts/forecastjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/jquery.cookie.js",
                "~/_GlobalResources/Js/RefreshCountDown.js",
                "~/_GlobalResources/Js/ErrorMsg/ErrorMsg.js",
                "~/_GlobalResources/Js/Core.js"));

            bundles.Add(new ScriptBundle("~/scripts/messagejs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/Core.js",
                "~/Views/Message/Resources/PortalPage.js"));

            bundles.Add(new ScriptBundle("~/scripts/logjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/Core.js",
                "~/Views/Log/Resources/monPickr-0.0.3.js",
                "~/Views/Log/Resources/Log.js"));

            bundles.Add(new ScriptBundle("~/scripts/accountjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery.cookie.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/Core.js"));

            bundles.Add(new ScriptBundle("~/scripts/custsettingjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/noty/jquery.noty.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/Core.js",
                "~/Views/Account/Resources/CustSettingEngine/Core.js",
                "~/Views/Account/Resources/CustSettingEngine/PTSetting.js",
                "~/Views/Account/Resources/CustSettingEngine/BettingLimit.js",
                "~/Views/Account/Resources/CustSettingEngine/BetType.js",
                "~/Views/Account/Resources/CustSettingEngine/BetTypeGroup.js",
                "~/Views/Account/Resources/CustSettingEngine/CommissionSetting.js"));

            bundles.Add(new ScriptBundle("~/scripts/accountlistjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/Core.js",
                "~/Views/Account/Resources/AccountList.js"));

            bundles.Add(new ScriptBundle("~/scripts/logloginjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery.cookie.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/Core.js"));

            bundles.Add(new ScriptBundle("~/scripts/treeviewjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery.cookie.js",
                "~/Views/Account/Resources/jquery.treeview.js",
                "~/Views/Account/Resources/jquery.treeview.async.js"));

            bundles.Add(new ScriptBundle("~/scripts/viewsettingjs").Include(
                "~/_GlobalResources/Js/head.js",
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/jquery-ui.js",
                "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
                "~/_GlobalResources/Js/Core.js"));

            bundles.Add(new ScriptBundle("~/scripts/unauthjs").Include(
                "~/_GlobalResources/Js/jquery.js",
                "~/_GlobalResources/Js/Core.js",
                "~/Views/Error/Resources/Unauthorized.js"));

            bundles.FileSetOrderList.Clear();
        }
    }
}