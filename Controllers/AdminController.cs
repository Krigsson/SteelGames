using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SteelGames.Models;

namespace SteelGames.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult MainAdminPanel()
        {
            ViewData["userModel"] = SteelGames.Models.User.getInstance();
            ViewData["GameModel"] = SteelGames.Models.GameList.getInstance();
            return View();
        }

        [HttpGet]
        public ActionResult AddGameAdminPanel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddGame()
        {
            var gameName = Request.Form["GameName"];
            var description = Request.Form["Description"];
            var price = double.Parse(Request.Form["Price"]);
            var categoryName = Request.Form["Category"];
            var platform = Request.Form["Platform"];
            var imageFolder = Request.Form["ImageFolder"];
            var previewImageFile = Request.Files["singleImage"];
            var OS = Request.Form["OS"];
            var processor = Request.Form["Processor"];
            var memory = Request.Form["Memory"];
            var graphics = Request.Form["Graphics"];
            var directX = Request.Form["DirectX"];
            var storage = Request.Form["Storage"];
            var soundCard = Request.Form["SoundCard"];

            SystemRequirements req = new SystemRequirements(OS, processor, memory, graphics, directX, storage, soundCard);

            Game newGame = new Game(gameName, platform, description, price, categoryName,
                                    previewImageFile.FileName, imageFolder, 0, req);

            if (previewImageFile != null && previewImageFile.ContentLength > 0)
            {
                var previewImageFileFileName = Path.GetFileName(previewImageFile.FileName);
                var previewImageFileFilePath = Path.Combine(Server.MapPath("~/source/images/preview"), previewImageFileFileName);
                previewImageFile.SaveAs(previewImageFileFilePath);
            }

            var gameImagesFiles = Request.Files.GetMultiple("multipleImages");

            if (gameImagesFiles != null && gameImagesFiles.Count > 0)
            {
                string temp = Server.MapPath("~/source/images/games/" + imageFolder);

                if (!Directory.Exists(temp))
                {
                    Directory.CreateDirectory(temp);
                }

                foreach (var image in gameImagesFiles)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/source/images/games/" + imageFolder), fileName);
                        image.SaveAs(filePath);
                    }
                }
            }

            DBConnector.getInstance().AddNewGameToDB(newGame);

            return RedirectToAction("MainAdminPanel", "Admin");
        }

        [HttpGet]
        public ActionResult AddKeysToGame(int gameID)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddKeysToGame()
        {
            int gameID = int.Parse(Request.Form["gameID"]);
            string keys = Request.Form["keyInput"];

            DBConnector.getInstance().AddKeysToGame(gameID, keys);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult EditGameAdminPanel(int gameID)
        {
            Game model = SteelGames.Models.GameList.getInstance()[gameID - 1];
            return View(model);
        }

        [HttpPost]
        public ActionResult ApplyEditedGame()
        {
            int gameID = int.Parse(Request.Form["gameID"]);
            string gameName = Request.Form["GameName"];
            string description = Request.Form["Description"];
            var price = double.Parse(Request.Form["Price"]);
            string categoryName = Request.Form["Category"];
            string platform = Request.Form["Platform"];
            string imageFolder = Request.Form["ImageFolder"];
            var previewImageFile = Request.Files["singleImage"];
            string OS = Request.Form["OS"];
            string processor = Request.Form["Processor"];
            string memory = Request.Form["Memory"];
            string graphics = Request.Form["Graphics"];
            string directX = Request.Form["DirectX"];
            string storage = Request.Form["Storage"];
            string soundCard = Request.Form["SoundCard"];
            bool newPreview = false;

            if (previewImageFile != null && previewImageFile.ContentLength > 0)
            {
                var previewImageFileFileName = Path.GetFileName(previewImageFile.FileName);
                var previewImageFileFilePath = Path.Combine(Server.MapPath("~/source/images/preview"), previewImageFileFileName);
                string oldPreviewPath = Path.Combine(Server.MapPath("~/source/images/preview"), 
                                        SteelGames.Models.GameList.getInstance()[gameID - 1].PreviewImageName);
                GeneralUtils.DeleteOldPreview(oldPreviewPath);
                previewImageFile.SaveAs(previewImageFileFilePath);
                newPreview = true;
            }

            var gameImagesFiles = Request.Files.GetMultiple("multipleImages");

            if (gameImagesFiles != null && gameImagesFiles.Count > 0)
            {
                string temp = Server.MapPath("~/source/images/games/" + imageFolder);

                if (!Directory.Exists(temp))
                {
                    Directory.CreateDirectory(temp);
                }

                foreach (var image in gameImagesFiles)
                {
                    if (image != null && image.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(image.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/source/images/games/" + imageFolder), fileName);
                        image.SaveAs(filePath);
                    }
                }
            }

            SystemRequirements req = new SystemRequirements(OS, processor, memory, graphics, directX, storage, soundCard);
            Game editedGame;
            if(newPreview)
            {
                editedGame = new Game(gameName, platform, description, price, categoryName,
                                    previewImageFile.FileName, imageFolder, 0, req);
            }
            else
            {
                editedGame = new Game(gameName, platform, description, price, categoryName,
                                      imageFolder, 0, req);
            }

            DBConnector.getInstance().EditGameAttributes(gameID, editedGame, newPreview);

            return RedirectToAction("MainAdminPanel", "Admin");
        }
    }
}