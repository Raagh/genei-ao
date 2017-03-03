using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AORNet.Model
{
    public class Player
    {
        public string Name { get; set; } = string.Empty;
        public string Faction { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public bool IsParalized { get; set; } = false;
        public bool IsInvisible { get; set; } = false;
        public int PosX { get; set; } = 0;
        public int PosY { get; set; } = 0;

        public Player(string name, string faction, string playerClass, bool isParalized, bool isInvisible, int posX, int posY )
        {
            Name = name;
            Faction = faction;
            Class = playerClass;
            IsParalized = isParalized;
            IsInvisible = isInvisible;
            PosX = posX;
            PosY = posY;
        }

        public Player()
        {
            
        }
    }
}
