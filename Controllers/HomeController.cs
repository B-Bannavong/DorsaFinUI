using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DorsaFinUI.Models;
using DorsaFinUI.Services;


namespace DorsaFinUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var model = WebApiService.getApiList<Option>("options/?lTicker=AAPL&lExpiration=2023-11-17", null);

            return View(/*model*/);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}