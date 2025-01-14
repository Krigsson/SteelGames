﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using OfficeOpenXml;

namespace SteelGames.Models
{
    abstract public class TemplateReportSaver
    {
        public string FileName { get; set; }

        public MemoryStream CreateReport()
        {
            ExcelPackage package = new ExcelPackage();
            MemoryStream stream = new MemoryStream();
            FileName = GenerateFileName();
            CreateHeaders(package);
            FillRows(package);
            package.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }

        protected abstract void CreateHeaders(ExcelPackage package);
        protected abstract void FillRows(ExcelPackage package);
        protected abstract string GenerateFileName();
    }

    public class TemplateGameReportSaver : TemplateReportSaver
    {
        public GameKeyModel Games { get; set; }

        public TemplateGameReportSaver(GameKeyModel games)
        {
            Games = games;
        }

        protected override void CreateHeaders(ExcelPackage package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Games and keys");
            worksheet.Cells["A1"].Value = "Name";
            worksheet.Cells["B1"].Value = "Platform";
            worksheet.Cells["C1"].Value = "Key";
        }

        protected override void FillRows(ExcelPackage package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int currentRow = 2;
            foreach(GameKeyModel.KeyStruct games in Games.keys)
            {
                string strRow = currentRow.ToString();
                worksheet.Cells["A" + strRow].Value = games.GameName;
                worksheet.Cells["B" + strRow].Value = games.Platform;
                worksheet.Cells["C" + strRow].Value = games.Key;
                currentRow++;
            }

        }

        protected override string GenerateFileName()
        {
            return "GameReport" +
                DateTime.Now.ToString("ddMMyyyy-HHmmss") +
                ".xlsx";
        }
    }

    public class TemplateAdminPRLReportSaver : TemplateReportSaver
    {
        public List<Game> Games { get; set; }

        public TemplateAdminPRLReportSaver(List<Game> games)
        {
            Games = games;
        }

        protected override void CreateHeaders(ExcelPackage package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Price list");
            worksheet.Cells["A1"].Value = "Name";
            worksheet.Cells["B1"].Value = "Platform";
            worksheet.Cells["C1"].Value = "Category";
            worksheet.Cells["D1"].Value = "Price";
            worksheet.Cells["E1"].Value = "Keys avaliable";
        }

        protected override void FillRows(ExcelPackage package)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int currentRow = 2;
            foreach (Game game in Games)
            {
                string strRow = currentRow.ToString();
                worksheet.Cells["A" + strRow].Value = game.Name;
                worksheet.Cells["B" + strRow].Value = game.Platform;
                worksheet.Cells["C" + strRow].Value = game.CategoryName;
                worksheet.Cells["D" + strRow].Value = game.Price;
                worksheet.Cells["E" + strRow].Value = DBConnector.getInstance().getAvailableKeysForCurrentGame(game.GameID);
                currentRow++;
            }

        }

        protected override string GenerateFileName()
        {
            return "GamesPriceList" +
                DateTime.Now.ToString("ddMMyyyy-HHmmss") +
                ".xlsx";
        }
    }
}