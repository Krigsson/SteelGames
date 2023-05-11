using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace SteelGames.Models
{
    public class Game
    {
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string CategoryName { get; set; }
        public string PreviewImageName { get; set; }
        public string ImageFolderName { get; set; }
        public int SystemReqID { get; set; }

        public SystemRequirements SysReq { get; set; }
    }

    public class DetailedGameModel
    {
        public Game GameDetails { get; set; }
        public List<string> Images { get; set; }
        public int AvaliableKeysForCurrentGame { get; set; }
        public void GetImages()
        {
            Images = Directory.GetFiles(GeneralUtils.PathToImages + GameDetails.ImageFolderName + "\\").ToList<string>();
            for(int i = 0; i < Images.Count; i++)
            {
                Images[i] = Images[i].Split('\\')[Images[i].Split('\\').Length - 1];
            }
        }

        public void GetAvaliableKeysCount()
        {
            DBConnector connector = DBConnector.getInstance();
            AvaliableKeysForCurrentGame = connector.getAvailableKeysForCurrentGame(GameDetails.GameID);
        }
    }
}