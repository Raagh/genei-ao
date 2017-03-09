using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AORNet.Configurations;
using AORNet.Model;
using EasyHook;

namespace AORNet.Helpers
{
    public static class CheatingHelper
    {
        #region -- Locals --

        public static List<Player> players = new List<Player>();

        #endregion

        #region -- Imports --

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(Keys vKeys);

        #endregion

        #region -- Functionality Methods --

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

        public static void SpeedHack()
        {
            if (GetAsyncKeyState(Keys.Up) == -32767)
            {
                PacketsHelper.SendToServer(GamePackets.MoveUp);
            }
            else if (GetAsyncKeyState(Keys.Down) == -32767)
            {
                PacketsHelper.SendToServer(GamePackets.MoveDown);
            }
            else if (GetAsyncKeyState(Keys.Left) == -32767)
            {
                PacketsHelper.SendToServer(GamePackets.MoveLeft);
            }
            else if (GetAsyncKeyState(Keys.Right) == -32767)
            {
                PacketsHelper.SendToServer(GamePackets.MoveRight);
            }
            PacketsHelper.SendToServer(GamePackets.RestartPositionUser);
        }

        public static void AutoAim()
        {
            if (GetAsyncKeyState(GeneiConfiguration.SwitchPlayerAutoAimKey) == -32767)
                SwitchPlayerInAutoAim();
            else if (GetAsyncKeyState(GeneiConfiguration.RemoKey) == -32767)
                CastSpell(Cheater.Configuration.RemoPosition, Cheater.Instance.PosX, Cheater.Instance.PosY);
            else
            {
                Player selectedPlayer = players.Find(x => x.IsSelected);
                if (GetAsyncKeyState(GeneiConfiguration.ApocaKey) == -32767)
                {
                    CastSpell(Cheater.Configuration.ApocaPosition, selectedPlayer.PosX, selectedPlayer.PosY);
                }
                else if (GetAsyncKeyState(GeneiConfiguration.DescargaKey) == -32767)
                {
                    CastSpell(Cheater.Configuration.DescargaPosition, selectedPlayer.PosX, selectedPlayer.PosY);
                }
                else if (GetAsyncKeyState(GeneiConfiguration.InmoKey) == -32767)
                {
                    CastSpell(Cheater.Configuration.InmoPosition, selectedPlayer.PosX, selectedPlayer.PosY);
                }
                else if (GetAsyncKeyState(GeneiConfiguration.TormentaKey) == -32767)
                {
                    CastSpell(Cheater.Configuration.TormentaPosition, selectedPlayer.PosX, selectedPlayer.PosY);
                }           
            }                       
        }

        public static void BorrarCartel()
        {
            PacketsHelper.SendToServer(GamePackets.CleanCartel);
        }

        #endregion

        #region -- Utility Methods --

        public static void CastSpell(string spellPosition, int posX, int posY)
        {
            PacketsHelper.SendToServer(GamePackets.CastSpell + spellPosition);
            PacketsHelper.SendToServer(GamePackets.IntermediateCastSpell);
            PacketsHelper.SendToServer(GamePackets.ThrowSpell + posX + "," + (posY - 1) + ",1");
            SendConsoleMessage("You casted tormenta on PosX:" + posX + " PosY:" + posY);
        }

        public static void SendConsoleMessage(string message)
        {
            PacketsHelper.SendToClient("||GeneiAO>" + message + "~10~236~18~0~0");
        }

        public static void ClearConsole()
        {
            for (int i = 0; i < 10; i++)
            {
                SendConsoleMessage("||            ~10~236~18~0~0");
            }
        }

        public static void SwitchPlayerInAutoAim()
        {
            List<Player> playersInRange = players.FindAll(x => x.InRange);

            if (playersInRange.Any())
            {
                Player selectedPlayer = playersInRange.Find(x => x.IsSelected);

                if (selectedPlayer != null && selectedPlayer != playersInRange.Last())
                {
                    var index = playersInRange.IndexOf(selectedPlayer);
                    Player nextPlayer = playersInRange[index + 1];
                    selectedPlayer.IsSelected = false;
                    nextPlayer.IsSelected = true;
                }
                else if (selectedPlayer != null && selectedPlayer == playersInRange.Last())
                {
                    selectedPlayer.IsSelected = false;
                    playersInRange.First().IsSelected = true;
                }
                else if (selectedPlayer == null)
                {
                    playersInRange.First().IsSelected = true;
                    selectedPlayer = playersInRange.First();
                }
                SendConsoleMessage("Selected player is: " + selectedPlayer.Name);
            }
            else if (!playersInRange.Any())
            {
                SendConsoleMessage("No characters in range!");
            }

        }

        #endregion


    }
}
