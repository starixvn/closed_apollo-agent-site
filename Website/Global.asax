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
		bundles.Add(new ScriptBundle("~/homejs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/Core.js",
			"~/_GlobalResources/Js/jquery.cookie.js",
			"~/Views/Home/Resources/Default.js"));

		bundles.Add(new ScriptBundle("~/reportjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery-ui.js",
			"~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
			"~/_GlobalResources/Js/noty/jquery.noty.js",
			"~/_GlobalResources/Js/Core.js",
			"~/Views/Reports/Resources/Reports.js"));

		bundles.Add(new ScriptBundle("~/outstandingjs").Include(
			"~/_GlobalResources/Js/Fanex.Grid/moment.js",
			"~/_GlobalResources/Js/Fanex.Grid/numeral.js",
			"~/_GlobalResources/Js/Fanex.Grid/Fanex.Grid.js"));

		bundles.Add(new ScriptBundle("~/totalbetsjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery-ui.js",
			"~/_Components/AGEWnd/AGEWnd.js",
			"~/_GlobalResources/Js/RefreshCountDown.js",
			"~/_GlobalResources/Js/Core.js",
			"~/Views/TotalBets/Resources/TotalBets.js"));

		bundles.Add(new ScriptBundle("~/forecastjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery-ui.js",
			"~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
			"~/_Components/AGEWnd/AGEWnd.js",
			"~/_GlobalResources/Js/RefreshCountDown.js",
			"~/_Components/ErrorMsg/ErrorMsg.js",
			"~/_GlobalResources/Js/Core.js"));
		
		bundles.Add(new ScriptBundle("~/messagejs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery-ui.js",
			"~/_Components/AGEWnd/AGEWnd.js",
			"~/_GlobalResources/Js/Core.js",
			"~/Views/Message/Resources/PortalPage.js"));

		bundles.Add(new ScriptBundle("~/logjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery-ui.js",
			"~/_Components/AGEWnd/AGEWnd.js",
			"~/_GlobalResources/Js/Core.js",
			"~/Views/Message/Resources/PortalPage.js"));

		bundles.Add(new ScriptBundle("~/accountjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery.cookie.js",
			"~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
			"~/_GlobalResources/Js/Core.js"));

		bundles.Add(new ScriptBundle("~/custsettingjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery-ui.js",
			"~/_GlobalResources/Js/noty/jquery.noty.min.js",
			"~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
			"~/_GlobalResources/Js/Core.js",
			"~/Views/Account/Resources/CustSettingEngine/Core.js",
			"~/Views/Account/Resources/CustSettingEngine/PTSetting.js",
			"~/Views/Account/Resources/CustSettingEngine/BettingLimit.js",
			"~/Views/Account/Resources/CustSettingEngine/BetType.js",
			"~/Views/Account/Resources/CustSettingEngine/BetTypeGroup.js",
			"~/Views/Account/Resources/CustSettingEngine/CommissionSetting.js"));

		bundles.Add(new ScriptBundle("~/accountlistjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery-ui.js",
			"~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
			"~/_GlobalResources/Js/Core.js",
			"~/Views/Account/Resources/AccountList.js"));

		bundles.Add(new ScriptBundle("~/logloginjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery.cookie.js",
			"~/_GlobalResources/Js/Core.js"));

		bundles.Add(new ScriptBundle("~/treeviewjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery.cookie.js",
			"~/Views/Account/Resources/jquery.treeview.js",
			"~/Views/Account/Resources/jquery.treeview.async.js"));

		bundles.Add(new ScriptBundle("~/viewsettingjs").Include(
			"~/_GlobalResources/Js/head.js",
			"~/_GlobalResources/Js/jquery.js",
			"~/_GlobalResources/Js/jquery-ui.js",
			"~/_GlobalResources/Js/BlockUI/jquery.blockUI.js",
			"~/_GlobalResources/Js/Core.js"));
	}

	void Application_Start(object sender, EventArgs e)
	{
        
        RouteTable.Routes.MapHubs(new Microsoft.AspNet.SignalR.HubConfiguration { EnableDetailedErrors = true });
        
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
			Server.ClearError();
			Response.Redirect(string.Format("{0}?err={1}", ApoURLs.URLs.Error, errorCode));
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
