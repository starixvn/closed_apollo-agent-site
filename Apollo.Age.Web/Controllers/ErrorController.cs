// --------------------------------------------------------------------------
// <copyright file="ErrorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis" date="2013.5.15">Created</change>
// </history>

namespace Apollo.Age.Web
{
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Resources;
    using Apollo.Age.Web.Services;

    /// <summary>
    /// Error controller class.
    /// </summary>
    public class ErrorController : Controller
    {
        private readonly UnderMaintenanceService _maintenanceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        public ErrorController()
        {
            _maintenanceService = new UnderMaintenanceService();
            if (SessionManager.CurrentSession != null)
            {
                LanguageManager.SetCulture(SessionManager.CurrentSession.Language);
            }
        }

        /// <summary>
        /// Indexes the specified error.
        /// </summary>
        /// <param name="err">Input error number.</param>
        /// <returns>Error message.</returns>
        public ActionResult Index(string err)
        {
            switch (err)
            {
                case "403":
                    ViewBag.ErrorMessage = Lang.HttpError_403;
                    break;

                case "404":
                    ViewBag.ErrorMessage = Lang.HttpError_404;
                    break;

                default:
                    ViewBag.ErrorMessage = Lang.HttpError_Default;
                    break;
            }

            return View();
        }

        /// <summary>
        /// Under maintenance action.
        /// </summary>
        /// <returns>Return view model or redirect to URL.</returns>
        public ActionResult UnderMaintenance()
        {
            var maintenanceInfo = _maintenanceService.MaintenanceInfo;

            if (maintenanceInfo.Status)
            {
                return View(maintenanceInfo);
            }
            else
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        /// <summary>
        /// Unauthorized action.
        /// </summary>
        /// <returns>Return error message.</returns>
        public ActionResult Unauthorized()
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Lang.UnauthorizedMsg);
            }

            return View();
        }

        /// <summary>
        /// Accesses the denied.
        /// </summary>
        /// <returns>View model.</returns>
        public ActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Accounts the stopped.
        /// </summary>
        /// <returns>View model.</returns>
        public ActionResult AccountStopped()
        {
            bool isInternal = false;
            if (SessionManager.CurrentSession != null)
            {
                isInternal = SessionManager.CurrentSession.IsInternalSignIn;
            }

            ViewBag.IsInternal = isInternal;
            return View();
        }

        /// <summary>
        /// Who am I action.
        /// </summary>
        /// <returns>View model.</returns>
        public ActionResult WhoAmI()
        {
            return View();
        }

        /// <summary>
        /// Redirects to under maintenance.
        /// </summary>
        /// <returns>View model.</returns>
        public ActionResult RedirectToUM()
        {
            return View();
        }
    }
}