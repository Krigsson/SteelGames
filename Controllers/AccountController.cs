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

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            DBConnector connector = DBConnector.getInstance();
            string email = form["email"];
            string password = form["password"];

            User loggedInUser = connector.loginUser(email, password);
            if (loggedInUser != null)
            {
                HttpContext.Session["LoggedInUser"] = loggedInUser;
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("InvalidLogin", "Account");
        }

        public ActionResult InvalidLogin()
        {
            return View();
        }

        public ActionResult AccountDetails()
        {
            GameKeyModel keyModel = GameKeyModel.getInstance();
            User currentUser = (User)HttpContext.Session["LoggedInUser"];
            keyModel.getKeys(currentUser.UserID);
            ViewData["UserModel"] = currentUser;
            ViewData["GameKeyModel"] = keyModel.keys;
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session["LoggedInUser"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}