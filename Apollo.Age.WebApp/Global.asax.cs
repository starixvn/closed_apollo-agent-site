// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Apollo.Age.WebApp
{
    using System;
    using System.Data.SqlClient;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Services;
    using Fanex.Data.Configuration;
    using Fanex.Logging;

    /// <summary>
    /// MVC application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbSettingProviderManager.Current.Start();
        }

        /// <summary>
        /// Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //// Logging
            //Exception ex = Server.GetLastError().GetBaseException();

            //if (!Request.IsLocal)
            //{
            //    // Get base exception data
            //    string message = ex.Message +
            //        "\r\nType: " + ex.GetType().ToString() +
            //        "\r\nSource: " + ex.Source +
            //        "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
            //        "\r\nForm: " + Request.Form.ToString() +
            //        "\r\nQueryString: " + Request.QueryString.ToString() +
            //        "\r\nTargetSite: " + ex.TargetSite +
            //        "\r\nStackTrace: " + ex.StackTrace;

            //    // Get additional exception data
            //    if (ex is SqlException)
            //    {
            //        message += "\r\nProcedure: " + ((SqlException)ex).Procedure +
            //            "\r\nLine Number: " + ((SqlException)ex).LineNumber +
            //            "\r\nServer: " + ((SqlException)ex).Server;
            //    }

            //    if (!(ex is HttpException))
            //    {
            //        LogService.Error(message);
            //    }
            //}

            //// Redirect to Error pages
            //var serverError = Server.GetLastError() as HttpException;
            //string errorCode = string.Empty;

            //// Do not log http exception
            //if (serverError != null)
            //{
            //    errorCode = serverError.GetHttpCode().ToString();
            //}

            //if (!Request.IsLocal)
            //{
            //    Server.ClearError();
            //    Response.Redirect(string.Format("{0}?err={1}", ApoURLs.URLs.Error, errorCode));
            //}
        }

        /// <summary>
        /// Handles the AcquireRequestState event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
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
                    && executeFileName.IndexOf(".png") == -1)
                {
                    var maintenanceService = new UnderMaintenanceService();

                    if (maintenanceService.MaintenanceInfo.Status)
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