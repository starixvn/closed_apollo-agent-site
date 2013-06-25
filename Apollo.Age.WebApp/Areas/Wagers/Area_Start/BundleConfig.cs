namespace Apollo.Age.WebApp.Areas.Wagers
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/Wagers").Include(
            ));
        }
    }
}