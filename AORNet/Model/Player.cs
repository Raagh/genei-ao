using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AORNet.Model
{
    public static class Player
    {
        public static string Name = string.Empty;
        public static string Faction = string.Empty;
        public static string Class = string.Empty;
        public static bool IsParalized = false;
        public static int PosX = 0;
        public static int PosY = 0;
        public static PlayerConfiguration Configuration = new PlayerConfiguration();
    }
}
