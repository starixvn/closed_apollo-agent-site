using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Apollo.Age.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return Redirect("~/Default.aspx");
        }

        public ActionResult Index2()
        {
            ViewBag.Message = "Demo Integrate MVC to Website.";

            return View(new DemoModel
            {
                ID = 1,
                Name = "Demo Name."
            });
        }

        public ActionResult AbsolutePath()
        {
            return View("~/Views/Home/AbsolutePath.cshtml");
        }
                
    }


    public class DemoModel
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }
    }

}
