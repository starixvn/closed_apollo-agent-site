// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Apollo.Age.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Apollo.Age.Web.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly UnderMaintenanceService maintenanceService;

        public HomeController()
        {
            maintenanceService = new UnderMaintenanceService();
        }

        public ActionResult Index()
        {
            return Redirect("~/Index.aspx");
        }

        public ActionResult UnderMaintenance()
        {
            var maintenanceInfo = maintenanceService.UnderMaintenanceInfo;
            if (maintenanceInfo.IsDownTime)
                return View(maintenanceInfo);
            else
                return Redirect("~/Index.aspx");
        }
    }
}