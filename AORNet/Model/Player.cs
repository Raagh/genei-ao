using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AORNet.Model
{
    public class Player
    {
        public string Name = string.Empty;
        public string Faction = string.Empty;
        public string Class = string.Empty;
        public bool IsParalized = false;
        public bool IsInvisible = false;
        public int PosX = 0;
        public int PosY = 0;
    }
}
