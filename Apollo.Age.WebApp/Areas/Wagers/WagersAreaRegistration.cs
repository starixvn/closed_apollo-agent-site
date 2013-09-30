namespace Apollo.Age.WebApp.Areas.Wagers
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    public class WagersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Wagers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}