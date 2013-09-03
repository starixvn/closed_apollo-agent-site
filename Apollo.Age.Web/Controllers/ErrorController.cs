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

    public class ErrorController : Controller
    {
        private readonly UnderMaintenanceService maintenanceService;

        public ErrorController()
        {
            maintenanceService = new UnderMaintenanceService();
            if (SessionManager.CurrentSession != null)
            {
                LanguageManager.SetCulture(SessionManager.CurrentSession.Language);
            }

        }

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

        public ActionResult UnderMaintenance()
        {
            var maintenanceInfo = maintenanceService.MaintenanceInfo;
           // return View(maintenanceInfo);

            if (maintenanceInfo.status)
            {
                return View(maintenanceInfo);
            }
            else
            {
                return Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        public ActionResult Unauthorized()
        {
            if (Request.IsAjaxRequest())
            {
                return new HttpUnauthorizedResult(Lang.UnauthorizedMsg);
            }

            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult AccountStopped()
        {
            return View();
        }

        public ActionResult WhoAmI()
        {
            return View();
        }

        public ActionResult RedirectToUM()
        {
            return View();
        }
    }
}