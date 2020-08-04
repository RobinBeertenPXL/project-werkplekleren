using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace PXL_Project_Team_05.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "XCustom-Header")]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
