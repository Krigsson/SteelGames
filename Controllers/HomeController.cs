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
        DBConnector connector = DBConnector.getInstance();
        GameList gameList = GameList.getInstance();
        public ActionResult Index()
        {
            gameList.Clear();
            gameList.AddRange(connector.getGamesByQuery("SELECT Game.*, SystemRequirements.* " +
                                                         "FROM Game " +
                                                         "JOIN SystemRequirements ON " +
                                                         "Game.SystemRequirementsID = SystemRequirements.SystemRequirementsID;"));
            ViewData["GameModel"] = gameList;
            ViewData["UserModel"] = SteelGames.Models.User.getInstance();
            return View();
        }

        public ActionResult GameDetails(int gameID)
        {
            DetailedGameModel model = new DetailedGameModel();
            model.GameDetails = gameList[gameID - 1];
            model.GetImages();
            model.GetAvaliableKeysCount();
            ViewData["GameModel"] = model;
            ViewData["UserModel"] = SteelGames.Models.User.getInstance();

            return View();
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