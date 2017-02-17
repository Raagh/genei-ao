using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AORNet.Model;

namespace AORNet.Helpers
{
    public static class CheatingHelper
    {
        public static void CastSpell(string spellPosition,int posX,int posY) 
        {
            Main.SendToServer(GamePackets.CAST_SPELL + spellPosition);

            Main.SendToServer(GamePackets.INTERMEDIATE_CAST_SPELL);

            Main.SendToServer(GamePackets.THROW_SPELL + "," + posX + "," + (posY -1) + "," + 1);

            Main.SendToServer(GamePackets.CLEAN_CARTEL);
        }

        public static void SendConsoleMessage(string message, string messageType)
        {
            Main.SendToClient("|| GeneiAO >"+ message + "~10~236~18~0~0");
        }

        public static void AutoPotas(string packet)
        {
            //TODO
        }
        
    }
}
