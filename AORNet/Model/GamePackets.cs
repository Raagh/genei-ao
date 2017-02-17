using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AORNet.Model
{
    public static class GamePackets
    {
        public const string CAST_SPELL = "LH";
        public const string INTERMEDIATE_CAST_SPELL = "UK1";
        public const string THROW_SPELL = "WLC";
        public const string MOVE_PLAYER = "MP";
        public const string PLAYER_ENTER_MAP = "RM";
        public const string PLAYER_EXIT_MAP = "QDL";
        public const string PLAYER_PARALIZED = "PU";
        public const string PLAYER_INVISIBILITY = "V3";
        public const string CHEATER_PARALIZED = "P9";
        public const string CHEATER_REMOVED = "P8";
        public const string CHEATER_EXIT_MAP = "CM";
        public const string INVENTORY_SPELL = "SHS";
        public const string INVENTORY_ITEM = "CSI";
        public const string INVENTORY_ITEM_TREASURE_ID = "24078";
        public const string MAP_ITEM = "HO";
        public const string GM_TAKING_PICTURE = "PAIN";
        public const string GM_READ_PROCESS = "PRC";
        public const string GM_READ_PROCESS_DEFINITION = "PRR";
        public const string CLEAN_CARTEL = ";1  ";

    }
}
