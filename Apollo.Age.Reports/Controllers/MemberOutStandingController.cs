namespace Apollo.Age.Reports.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Reports.ViewModels;

    public class MemberOutStandingController : Controller
    {
        private readonly OutStandingService _reportService;

        public MemberOutStandingController() : this(null) { }

        public MemberOutStandingController(OutStandingService reportService = null)
        {
            _reportService = reportService ?? new OutStandingService();
        }

        public ActionResult Index(MemberOutStandingCriteria criteria)
        {
            // call service to get report data here
            IEnumerable<OutStandingViewModel> models = _reportService.GetReportData<OutStandingViewModel>(criteria);

            // Add total row
            OutStandingViewModel totalModel = _reportService.Total(models);
            models.ToList().Add(totalModel);

            return View(models);
        }
    }
}