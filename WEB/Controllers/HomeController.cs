using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Projects()
        {
            ViewBag.Message = "Zoznam projektov";
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Prihlásie do aplikácie";
            return View();
        }

        public void Logout()
        {
            Session.RemoveAll();
            Response.Redirect($"{ConfigurationManager.AppSettings["webUrl"].ToString()}Home/Login");
        }
    }
}