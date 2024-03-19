using MySql.Data.MySqlClient;
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

        public SystemRequirements SystemRequirements
        {
            get => default;
            set
            {
            }
        }

        public Game() { }
        public Game(string name, string platform, string description, double price, string categoryName,
                    string previewImageFolder, string imageFolderName, int systemReqID, SystemRequirements sysreq)
        {
            Name = name;
            Platform = platform;
            Description = description;
            Price = price;
            CategoryName = categoryName;
            PreviewImageName = previewImageFolder;
            ImageFolderName = imageFolderName;
            SystemReqID = systemReqID;
            SysReq = sysreq;
        }

        public Game(string name, string platform, string description, double price, string categoryName,
                     string imageFolderName, int systemReqID, SystemRequirements sysreq)
        {
            Name = name;
            Platform = platform;
            Description = description;
            Price = price;
            CategoryName = categoryName;
            ImageFolderName = imageFolderName;
            SystemReqID = systemReqID;
            SysReq = sysreq;
        }
    }

    public class DetailedGameModel
    {
        public Game GameDetails { get; set; }
        public List<string> Images { get; set; }
        public int AvaliableKeysForCurrentGame { get; set; }
        public void GetImages()
        {
            Images = Directory.GetFiles(GeneralUtils.PathToImages + GameDetails.ImageFolderName + "\\").ToList<string>();
            for (int i = 0; i < Images.Count; i++)
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

    public class GameKeyModel
    {
        private static GameKeyModel instance;
        public List<KeyStruct> keys;

        private GameKeyModel()
        {
            if(keys == null)
            {
                keys = new List<KeyStruct>();
            }
        }

        public static GameKeyModel getInstance()
        {
            if (instance == null)
            {
                instance = new GameKeyModel();
            }

            return instance;
        }

        public class KeyStruct
        {
            public string Key { get; set; }
            public string GameName { get; set; }
            public string Platform { get; set; }
            public int GameID { get; set; }

            public KeyStruct(int gameID, string gameName, string key, string platform)
            {
                Key = key;
                GameName = gameName;
                GameID = gameID;
                Platform = platform;
            }
        }

        public void getKeys(int userID)
        {
            if(keys != null)
            {
                keys.Clear();
            }

            string query = "SELECT k.KeyValue, k.GameID, g.Name, g.Platform " +
                "FROM Purchase p " +
                "JOIN keys_t k ON p.KeyID = k.KeyID " +
                "JOIN game g ON k.GameID = g.GameID " +
                $"WHERE p.UserID = {userID};";

            DBConnector connector = DBConnector.getInstance();

            using(MySqlCommand command = new MySqlCommand(query, connector.databaseConnection))
            {
                MySqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        int gameID = int.Parse(reader["GameID"].ToString());
                        string keyValue = reader["KeyValue"].ToString();
                        string gameName = reader["Name"].ToString();
                        string platform = reader["Platform"].ToString();

                        KeyStruct key = new KeyStruct(gameID, gameName, keyValue, platform);

                        keys.Add(key);
                    }
                }

                reader.Close();
            }
        }
    }
}