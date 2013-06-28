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
            "~/_GlobalResources/Js/Core.js"
            ));

            bundles.Add(new ScriptBundle("~/areas/Wagers/BkNFLRugby").Include(
            "~/_GlobalResources/Js/head.js",
            "~/_GlobalResources/Js/jquery.js",
            "~/_GlobalResources/Js/Core.js",
            "~/Areas/Wagers/Views/BkNFLRugby/Resources/BkNFLRugby.js"
            ));

            bundles.Add(new ScriptBundle("~/areas/Wagers/Soccer").Include(
           "~/_GlobalResources/Js/head.js",
           "~/_GlobalResources/Js/jquery.js",
           "~/_GlobalResources/Js/Core.js",
           "~/Areas/Wagers/Views/Soccer/Resources/Soccer.js"
           ));
        }
    }
}