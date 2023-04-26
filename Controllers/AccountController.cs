using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SteelGames.Models;

namespace SteelGames.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            DBConnector connector = DBConnector.getInstance();
            string email = form["email"];
            string password = form["password"];
            string phone = form["phone"];
            connector.registerNewUser(email, password, phone);
            return RedirectToAction("Index", "Home");
        }
    }
}