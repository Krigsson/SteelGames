using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SteelGames.Models;

namespace SteelGames.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new GameModel();
            DBConnector connector = DBConnector.getInstance();
            List<Game> games = connector.getGamesByQuery("SELECT * FROM Game");

            model.Games = games;
            return View(model);
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