// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Apollo.Age.Web
{
    using System.Web.Mvc;
    using Apollo.Age.Web.Services;

    public class HomeController : BaseController
    {
        private const string DEFAULT_PAGE_URL = "~/Default.aspx";

        private readonly UnderMaintenanceService maintenanceService;

        public HomeController()
        {
            maintenanceService = new UnderMaintenanceService();
        }

        public ActionResult Index()
        {
            return Redirect(DEFAULT_PAGE_URL);
        }

        public ActionResult UnderMaintenance()
        {
            var maintenanceInfo = maintenanceService.MaintenanceInfo;

            if (maintenanceInfo.IsDownTime)
            {
                return View(maintenanceInfo);
            }
            else
            {
                return Redirect(DEFAULT_PAGE_URL);
            }
        }
    }
}