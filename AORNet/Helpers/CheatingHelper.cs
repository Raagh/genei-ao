using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AORNet.Configurations;
using AORNet.Model;

namespace AORNet.Helpers
{
    public static class CheatingHelper
    {
        public static void CastSpell(string spellPosition,int posX,int posY) 
        {
            Main.SendToServer(GamePackets.CastSpell + spellPosition);
            Main.SendToServer(GamePackets.IntermediateCastSpell);
            Main.SendToServer(GamePackets.ThrowSpell + "," + posX + "," + (posY -1) + "," + 1);
            Main.SendToServer(GamePackets.CleanCartel);
        }

        public static void SendConsoleMessage(string message)
        {
            Main.SendToClient("|| GeneiAO >"+ message + "~10~236~18~0~0");
        }

        public static void AutoPotas(string packet)
        {
            bool isProcessOpen = MemoryHelper.IsProcessOpen(LibraryConfiguration.ServerName);
            if (isProcessOpen)
            {
                IntPtr processHandle = new IntPtr();
                Process process = Process.GetProcessesByName(LibraryConfiguration.ServerName)[0];
                processHandle = MemoryHelper.OpenProcess(LibraryConfiguration.OpenForReading, false, process.Id); 
                int maxLife = MemoryHelper.Read(processHandle, LibraryConfiguration.StructAddress);
                int actualLife = MemoryHelper.Read(processHandle, LibraryConfiguration.StructAddress + 4);
                int maxMana = MemoryHelper.Read(processHandle, LibraryConfiguration.StructAddress + 8);
                int actualMana = MemoryHelper.Read(processHandle, LibraryConfiguration.StructAddress + 12);

                if (actualLife != 0)
                {
                    if (actualLife != maxLife)
                    {
                        Main.SendToServer(GamePackets.InventoryUsa1Slot);
                        Main.SendToServer(GamePackets.InventoryUse1Slot);
                    }
                    else if (actualMana != maxMana)
                    {
                        Main.SendToServer(GamePackets.InventoryUsa2Slot);
                        Main.SendToServer(GamePackets.InventoryUse2Slot);
                    }
                }
            }
        }

        public static void UseRedPotions()
        {
            Main.SendToServer("USA>O=:");
            Main.SendToServer("USAm~AA");
        }
        
    }
}
