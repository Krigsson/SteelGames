using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SteelGames.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessRegistration()
        {
            string email = Request.Form["email"];
            return Redirect("/Home/Index");
        }
    }
}