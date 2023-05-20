using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SteelGames.Models
{
    public class SystemRequirements
    {
        public int SystemReqID { get; set; }
        public string OS { get; set; }
        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Graphics { get; set; }
        public string DirectX { get; set; }
        public string Storage { get; set; }
        public string SoundCard { get; set; }

        public SystemRequirements() { }
        public SystemRequirements(string os, string processor, string memory, string graphics, string directX,
                                  string storage, string soundcard)
        {
            OS = os;
            Processor = processor;
            Memory = memory;
            Graphics = graphics;
            DirectX = directX;
            Storage = storage;
            SoundCard = soundcard;
        }
    }
}