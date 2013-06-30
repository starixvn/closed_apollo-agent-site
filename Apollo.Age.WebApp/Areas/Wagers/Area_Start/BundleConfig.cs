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
            "~/_GlobalResources/Js/Core.js",
            "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
             "~/Areas/Wagers/Scripts/Wagers.js"
            ));

        }
    }
}