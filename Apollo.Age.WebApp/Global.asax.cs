using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Apollo.Age.Web;
using Apollo.Age.Web.Services;
using Fanex.Data.Configuration;
using Fanex.Logging;

namespace Apollo.Age.WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static UnderMaintenanceService maintenanceService = null;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            maintenanceService = new UnderMaintenanceService();
            DbSettingProviderManager.Current.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
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

            // Do not log http exception
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

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                var executeFileName = Request.AppRelativeCurrentExecutionFilePath;

                if (executeFileName.IndexOf("RedirectToUM") == -1
                    && executeFileName.IndexOf("UnderMaintenance") == -1
                    && executeFileName.IndexOf(".less") == -1
                    && executeFileName.IndexOf(".css") == -1
                    && executeFileName.IndexOf(".js") == -1
                    && executeFileName.IndexOf(".gif") == -1
                    && executeFileName.IndexOf(".png") == -1
                    )
                {
                    if (maintenanceService.MaintenanceInfo.status)
                    {
                        Response.Redirect(ApoURLs.URLs.RedirectToUM, false);
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }
        }
    }
}