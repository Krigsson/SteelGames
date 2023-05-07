﻿using System;
using System.Collections.Generic;
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
        public int SystemReqID { get; set; }

        public SystemRequirements SysReq { get; set; }
    }

    public class GameModel
    {
        public List<Game> Games { get; set; }
    }
}