using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteelGames.Models
{
    public class GameList : List<Game>
    {
        private static GameList instance;
        private GameList() { }

        public static GameList getInstance()
        {
            if (instance == null)
            {
                instance = new GameList();
            }

            return instance;
        }

        public static void UpdateGames()
        {
            if (instance != null)
            {

                instance.Clear();
                instance.AddRange(DBConnector.getInstance().
                                    getGamesByQuery("SELECT Game.*, SystemRequirements.* " +
                                    "FROM Game " +
                                    "JOIN SystemRequirements ON " +
                                    "Game.SystemRequirementsID = SystemRequirements.SystemRequirementsID;"));
            }
        }
    }
}