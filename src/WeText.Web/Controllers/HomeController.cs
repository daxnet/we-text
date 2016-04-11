using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeText.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "WeText - A demonstration of CQRS and Microservice architecture.";

            return View();
        }

        public ActionResult Info(string messageTitle, string messageText, string returnAction = null, string returnController = null)
        {
            if (string.IsNullOrEmpty(messageTitle) || string.IsNullOrEmpty(messageText))
                return RedirectToAction("Index", "Home");
            ViewBag.MessageTitle = messageTitle;
            ViewBag.MessageText = messageText;
            ViewBag.ReturnAction = returnAction;
            ViewBag.ReturnController = returnController;
            return View();
        }
    }
}