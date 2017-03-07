using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AORNet.Model
{
    public class Player
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public Factions Faction { get; set; }
        public string Class { get; set; } = string.Empty;
        public bool IsParalized { get; set; } = false;
        public bool IsInvisible { get; set; } = false;
        public int PosX { get; set; } = 0;
        public int PosY { get; set; } = 0;
        public bool InRange { get; set; } = false;
        public bool IsSelected { get; set; } = false;

        public Player(int Id, string name, Factions faction, string playerClass, bool isParalized, bool isInvisible, int posX, int posY, bool inRange, bool isSelected )
        {
            ID = Id;
            Name = name;
            Faction = faction;
            Class = playerClass;
            IsParalized = isParalized;
            IsInvisible = isInvisible;
            PosX = posX;
            PosY = posY;
            InRange = inRange;
            IsSelected = isSelected;
        }

        public Player()
        {
            
        }

        public enum Factions
        {
            Criminal = 3,
            Ciudadano = 2,
            Neutral = 4
        }
    }
}
