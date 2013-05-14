<%@ Application Language="C#" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.SessionState" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Apollo.Age.Web.Services" %>
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

        routes.MapRoute(
             "Default", // Route name
             "{controller}/{action}/{id}", // URL with parameters
             new { controller = "Home", action = "Index", id = UrlParameter.Optional }
         );
    }

    void Application_Start(object sender, EventArgs e)
    {
        maintenanceService = new UnderMaintenanceService();
        AreaRegistration.RegisterAllAreas();
        RegisterGlobalFilters(GlobalFilters.Filters);
        RegisterRoutes(RouteTable.Routes);

        Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();

    }

    void AppSettings_LoadComplete(object sender, bool error)
    {
        if (error) return;

        TplCore.TplSettings.UrlVersion = ConfigurationManager.AppSettings["JsVersion"];
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

            Fanex.Logging.LogService.Error(message);
        }

        // Redirect to Error pages
        var serverError = Server.GetLastError() as HttpException;
        string errorCode = string.Empty;

        if (serverError != null)
        {
            errorCode = serverError.GetHttpCode().ToString();
        }

        Server.ClearError();
        Response.Redirect("~/Home/Error?err=" + errorCode);
    }

    void Application_AcquireRequestState(object sender, EventArgs e)
    {

        try
        {
            var executeFileName = Request.AppRelativeCurrentExecutionFilePath;
            if (executeFileName.IndexOf("UnderMaintenance") == -1 && executeFileName.IndexOf("GetMessage") == -1)
            {
                var maintenanceInfo = maintenanceService.MaintenanceInfo;
                if (maintenanceInfo.IsDownTime)
                    Response.Redirect("~/Home/UnderMaintenance");
            }
        }
        catch (Exception ex)
        {
            Fanex.Logging.LogService.Error(ex.Message, ex);
        }

    }

</script>