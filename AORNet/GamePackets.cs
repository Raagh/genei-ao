using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AORNet
{
    public static class GamePackets
    {
        public static readonly string CAST_SPELL = "LH";
        public static readonly string INTERMEDIATE_CAST_SPELL = "UK1";
        public static readonly string THROW_SPELL = "WLC";
        public static readonly string MOVE_PLAYER = "MP";
        public static readonly string PLAYER_ENTER_MAP = "RM";
        public static readonly string PLAYER_EXIT_MAP = "QDL";
        public static readonly string PLAYER_PARALIZED = "PU";
        public static readonly string PLAYER_INVISIBILITY = "V3";
        public static readonly string CHEATER_PARALIZED = "P9";
        public static readonly string CHEATER_REMOVED = "P8";
        public static readonly string CHEATER_EXIT_MAP = "CM";
        public static readonly string INVENTORY_SPELL = "SHS";
        public static readonly string INVENTORY_ITEM = "CSI";
        public static readonly string MAP_ITEM = "HO";
        public static readonly string GM_TAKING_PICTURE = "PAIN";
        public static readonly string GM_READ_PROCESS = "PRC";
        public static readonly string GM_READ_PROCESS_DEFINITION = "PRR";
    }
}
