<%@ Application Language="C#" %>
<%@ Import Namespace="System.Xml" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.SessionState" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Sunplus.Agent.Utilities" %>
<%@ Import Namespace="Sunplus.Agent.Model.Entities" %>
<%@ Import Namespace="Sunplus.Agent.Model.BL.Authorization" %>
<%@ Import Namespace="Apollo.Age.Web.Services" %>
<script RunAt="server">

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

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError().GetBaseException();
        if (ex is HttpException)
        {
            return;
        }

        string message = ex.Message +
                        "\r\nSOURCE: " + ex.Source +
                        "\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                        "\r\nFORM: " + Request.Form.ToString() +
                        "\r\nQUERYSTRING: " + Request.QueryString.ToString() +
                        "\r\nTARGETSITE: " + ex.TargetSite +
                        "\r\nSTACKTRACE: " + ex.StackTrace;

        if (HttpContext.Current.Handler is IRequiresSessionState)
        {
            IUserSession session = UserSession.Current;
            message += "\r\nCUSTID:" + session.CustId;
        }

        Logs.Write(message);

        if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.LocalTest)
        {
            Server.ClearError();
            Server.Transfer("~/Error.aspx");
        }
    }

    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        var executeFileName = Request.AppRelativeCurrentExecutionFilePath;
        if (executeFileName.IndexOf("UnderMaintenance") == -1)
        {
            UnderMaintenanceService maintenanceService = new UnderMaintenanceService();
            var maintenanceInfo = maintenanceService.UnderMaintenanceInfo;
            if (maintenanceInfo.IsDownTime)
                Response.Redirect("~/Home/UnderMaintenance");
        }

    }

    private void GetRequestParams(ref int custId, ref long transId, ref int masterId, out bool isDataWarehouse, out string url)
    {
        int.TryParse(Request["custid"], out custId);
        if (custId == 0) int.TryParse(Request["cid"], out custId);
        if (custId == 0) int.TryParse(Request["recommend"], out custId);
        if (custId == 0) int.TryParse(Request["mrecommend"], out custId);
        if (custId == 0) int.TryParse(Request["idagent"], out custId);
        long.TryParse(Request["transid"], out transId);

        if (transId == 0) long.TryParse(Request["refno"], out transId);
        int.TryParse(Request["idmaster"], out masterId);

        isDataWarehouse = (Request["iswhcheck"] == "1");

        // Get file path (remove /)
        url = Request.Path.Remove(0, 1);
        url = url.ToLower();
    }

    void Session_Start(object sender, EventArgs e)
    {

    }

    void Session_End(object sender, EventArgs e)
    {

    }

</script>