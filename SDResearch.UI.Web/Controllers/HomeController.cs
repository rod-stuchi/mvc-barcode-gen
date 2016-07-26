using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SDResearch.UI.Web.Controllers
{
    public class HomeController : Controller
    {
        //[OutputCache(Duration = 3600)]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            return View();
        }
    }
}