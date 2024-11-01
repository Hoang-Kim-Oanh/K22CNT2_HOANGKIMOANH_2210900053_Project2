using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        // Define the database context inside the class
        K22CNT2_HOANGKIMOANH_2210900053Entities db = new K22CNT2_HOANGKIMOANH_2210900053Entities();

        public ActionResult Index()
        {
            return View(db.SAN_PHAM.ToList());
        }

        public ActionResult Details()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Giohang()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult Menu()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
