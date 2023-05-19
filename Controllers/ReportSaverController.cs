using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SteelGames.Models;

namespace SteelGames.Controllers
{
    public class ReportSaverController : Controller
    {
        public ActionResult SaveGamesReportXls()
        {
            GameKeyModel keyModel = GameKeyModel.getInstance();
            TemplateReportSaver reportSaver = new TemplateGameReportSaver(keyModel);

            MemoryStream stream = reportSaver.CreateReport();
            stream.Position = 0;

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportSaver.FileName);
        }
    }
}