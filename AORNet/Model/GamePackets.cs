using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AORNet.Model
{
    public static class GamePackets
    {
        public const string CastSpell = "LH";
        public const string IntermediateCastSpell = "UK1";
        public const string ThrowSpell = "WLC";
        public const string MovePlayer = "MP";
        public const string PlayerEnterMap = "RM";
        public const string PlayerExitMap = "QDL";
        public const string PlayerParalized = "PU";
        public const string PlayerInvisibility = "V3";
        public const string CheaterParalized = "P9";
        public const string CheaterRemoved = "P8";
        public const string CheaterExitMap = "CM";
        public const string InventorySpell = "SHS";
        public const string InventoryItem = "CSI";
        public const string MapItemTreasureId = "24078";
        public const string MapItem = "HO";
        public const string GmTakingPicture = "PAIN";
        public const string GmReadProcess = "PRC";
        public const string GmReadProcessDefinition = "PRR";
        public const string CleanCartel = ";1  ";
        public const string UseItemClick = "USE";
        public const string UseItemKey = "USA";
    }
}
