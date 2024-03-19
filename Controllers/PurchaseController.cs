using System.Web.Mvc;
using SteelGames.Models;

namespace SteelGames.Controllers
{
    public class PurchaseController : Controller
    {
        [HttpGet]
        public ActionResult BuyGame()
        {
            ViewData["UserModel"] = HttpContext.Session["LoggedInUser"];
            ViewData["GameModel"] = (Game)HttpContext.Session["CurrentGame"];
            return View();
        }

        [HttpPost]
        public ActionResult BuyGame(FormCollection form)
        {
            string cardNumber = form["cardNumber"];
            double price = double.Parse(form["gamePrice"]);
            int gameID = int.Parse(form["gameID"]);

            ProcessPurchase.processPurchase(cardNumber, price, gameID);

            return RedirectToAction("Index", "Home");
        }
    }
}