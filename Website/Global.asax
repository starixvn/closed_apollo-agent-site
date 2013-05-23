<%@ Application Language="C#" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.SessionState" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Apollo.Age.Web.Services" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="Fanex.Data.Configuration" %>
<%@ Import Namespace="Fanex.Logging" %>
<%@ Import Namespace="Apollo.Age.Web" %>
<script RunAt="server">

    public static UnderMaintenanceService maintenanceService = null;

    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
    }

    static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
        routes.IgnoreRoute("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });
        routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

        routes.MapRoute("AccessDenied", "AccessDenied", new { controller = "Error", action = "AccessDenied" });
        routes.MapRoute("AccountStopped", "AccountStopped", new { controller = "Error", action = "AccountStopped" });
        routes.MapRoute("Error", "Error", new { controller = "Error", action = "Index" });
        routes.MapRoute("Unauthorized", "Unauthorized", new { controller = "Error", action = "Unauthorized" });
        routes.MapRoute("UnderMaintenance", "UnderMaintenance", new { controller = "Error", action = "UnderMaintenance" });

        routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
    }

    static void RegisterBundles(BundleCollection bundles)
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
            "~/_GlobalResources/Js/RefreshCountDown.js",
            "~/_GlobalResources/Js/Core.js",
            "~/Views/TotalBets/Resources/TotalBets.js"));

        bundles.Add(new ScriptBundle("~/scripts/forecastjs").Include(
            "~/_GlobalResources/Js/head.js",
            "~/_GlobalResources/Js/jquery.js",
            "~/_GlobalResources/Js/jquery-ui.js",
            "~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
            "~/_GlobalResources/Js/RefreshCountDown.js",
            "~/_GlobalResources/Js/ErrorMsg/ErrorMsg.js",
            "~/_GlobalResources/Js/Core.js"));

        bundles.Add(new ScriptBundle("~/scripts/messagejs").Include(
            "~/_GlobalResources/Js/head.js",
            "~/_GlobalResources/Js/jquery.js",
            "~/_GlobalResources/Js/jquery-ui.js",
            "~/_GlobalResources/Js/Core.js",
            "~/Views/Message/Resources/PortalPage.js"));

        bundles.Add(new ScriptBundle("~/scripts/logjs").Include(
            "~/_GlobalResources/Js/head.js",
            "~/_GlobalResources/Js/jquery.js",
            "~/_GlobalResources/Js/jquery-ui.js",
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
    }

    void Application_Start(object sender, EventArgs e)
    {
        
        maintenanceService = new UnderMaintenanceService();
        AreaRegistration.RegisterAllAreas();
        RegisterGlobalFilters(GlobalFilters.Filters);

        RegisterRoutes(RouteTable.Routes);
        RegisterBundles(BundleTable.Bundles);

        DbSettingProviderManager.Current.Start();
    }

    /// <summary>
    /// Code that runs when an unhandled error occurs
    /// </summary>
    void Application_Error(object sender, EventArgs e)
    {
        // Logging
        Exception ex = Server.GetLastError().GetBaseException();

        if (!Request.IsLocal)
        {
            string message = ex.Message +
                "\r\nSource: " + ex.Source +
                "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                "\r\nForm: " + Request.Form.ToString() +
                "\r\nQueryString: " + Request.QueryString.ToString() +
                "\r\nTargetSite: " + ex.TargetSite +
                "\r\nStackTrace: " + ex.StackTrace;

            LogService.Error(message);
        }

        // Redirect to Error pages
        var serverError = Server.GetLastError() as HttpException;
        string errorCode = string.Empty;

        if (serverError != null)
        {
            errorCode = serverError.GetHttpCode().ToString();
        }

        if (!Request.IsLocal)
        {
            //Server.ClearError();
            //Response.Redirect(string.Format("{0}?err={1}", ApoURLs.URLs.Error, errorCode));
        }
    }

    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        try
        {
            var executeFileName = Request.AppRelativeCurrentExecutionFilePath;

            if (executeFileName.IndexOf("UnderMaintenance") == -1
                && executeFileName.IndexOf("GetMessage") == -1)
            {
                var maintenanceInfo = maintenanceService.MaintenanceInfo;

                if (maintenanceInfo.IsDownTime)
                {
                    Response.Redirect(ApoURLs.URLs.UnderMaintenance);
                }
            }
        }
        catch (Exception ex)
        {
            LogService.Error(ex.Message, ex);
        }

    }

</script>
