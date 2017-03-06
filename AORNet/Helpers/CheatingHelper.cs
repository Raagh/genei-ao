using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AORNet.Configurations;
using AORNet.Model;
using EasyHook;

namespace AORNet.Helpers
{
    public static class CheatingHelper
    {
        public static void CastSpell(string spellPosition,int posX,int posY) 
        {
            PacketsHelper.SendToServer(GamePackets.CastSpell + spellPosition);
            PacketsHelper.SendToServer(GamePackets.IntermediateCastSpell);
            PacketsHelper.SendToServer(GamePackets.ThrowSpell + "," + posX + "," + (posY -1) + "," + 1);
            PacketsHelper.SendToServer(GamePackets.CleanCartel);
        }

        public static void SendConsoleMessage(string message)
        {
            PacketsHelper.SendToClient("||GeneiAO>"+ message + "~10~236~18~0~0");
        }

        public static void AutoPotas()
        {          
            if (LibraryConfiguration.IsProcessOpen)
            {
                int maxLife = MemoryHelper.Read(LibraryConfiguration.ProcessHandle, LibraryConfiguration.StructAddress);
                int actualLife = MemoryHelper.Read(LibraryConfiguration.ProcessHandle, LibraryConfiguration.StructAddress + 4);
                int maxMana = MemoryHelper.Read(LibraryConfiguration.ProcessHandle, LibraryConfiguration.StructAddress + 8);
                int actualMana = MemoryHelper.Read(LibraryConfiguration.ProcessHandle, LibraryConfiguration.StructAddress + 12);

                if (actualLife != 0)
                {
                    if (actualLife != maxLife)
                    {
                        PacketsHelper.SendToServer(GamePackets.UseItemClick + PacketsHelper.Encrypt(Cheater.Configuration.RedPotionPosition));
                        PacketsHelper.SendToServer(GamePackets.UseItemKey + PacketsHelper.Encrypt(Cheater.Configuration.RedPotionPosition));
                    }
                    else if (actualMana != maxMana)
                    {
                        PacketsHelper.SendToServer(GamePackets.UseItemClick + PacketsHelper.Encrypt(Cheater.Configuration.BluePotionPosition));
                        PacketsHelper.SendToServer(GamePackets.UseItemKey + PacketsHelper.Encrypt(Cheater.Configuration.BluePotionPosition));
                    }
                }
            }
            else
            {
                LibraryConfiguration.IsProcessOpen = MemoryHelper.IsProcessOpen(LibraryConfiguration.ServerName);
                LibraryConfiguration.AOProcess = Process.GetProcessesByName(LibraryConfiguration.ServerName)[0];
                LibraryConfiguration.ProcessHandle = MemoryHelper.OpenProcess(LibraryConfiguration.OpenForReading, false, LibraryConfiguration.AOProcess.Id);
            }
        }

        public static void AutoRemo()
        {
            if (Cheater.Instance.IsParalized)
            {
                CastSpell(Cheater.Configuration.RemoPosition, Cheater.Instance.PosX, Cheater.Instance.PosY);
            }
        }       
    }
}
