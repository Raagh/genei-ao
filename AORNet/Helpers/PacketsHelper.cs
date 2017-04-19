﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AORNet.Configurations;
using AORNet.Model;

namespace AORNet.Helpers
{
    public static class PacketsHelper
    {
        public static void AnalyzeHandlePackets(string packet)
        {

            //
            //// If the GameMaster is taking a screenshot of you
            //
            if (packet.StartsWith(GamePackets.GmTakingPicture))
                CheatingHelper.ClearConsole();
            //
            //// Checks if there is any Chest in the map
            //
            else if (packet.StartsWith(GamePackets.MapItem))
            {
                string[] split = packet.Split(',');
                string itemId = split[0];
                itemId = itemId.Substring(2);

                if (itemId.Contains(GamePackets.MapItemTreasureId))
                {
                    CheatingHelper.SendConsoleMessage("Cofre encontrado en x:" + split[1] + " y: " + split[2]);
                }
            }
            //
            //// Checks for potions in the inventory
            //
            else if (packet.StartsWith(GamePackets.InventoryItem))
            {
                string[] split = packet.Split(',');
                string inventoryPosition = split[0].Substring(3);
                string itemId = split[4];
                if (itemId.Contains(GeneiConfiguration.RedPotionId))
                {
                    Cheater.Configuration.RedPotionPosition = inventoryPosition;
                }
                else if (itemId.Contains(GeneiConfiguration.BluePotionId))
                {
                    Cheater.Configuration.BluePotionPosition = inventoryPosition;
                }
                else if (itemId.Contains(GeneiConfiguration.YellowPotionId))
                {
                    Cheater.Configuration.YellowPotionPosition = inventoryPosition;
                }
            }
            //
            //// Updates player Status
            //
            else if (packet.StartsWith(GamePackets.CheaterStatus))
            {
                string[] split = packet.Split(',');
                int maxLife = int.Parse(split[0].Substring(3));
                int actualLife = int.Parse(split[1]);
                int maxMana = int.Parse(split[2]);
                int actualMana = int.Parse(split[3]);

                Cheater.MaxLife = maxLife;
                Cheater.ActualLife = actualLife;
                Cheater.MaxMana = maxMana;
                Cheater.ActualMana = actualMana;
            }
            //
            //// Updates player life
            //
            else if (packet.StartsWith(GamePackets.CheaterUpdateLife))
            {
                int actualLife = int.Parse(packet.Substring(2));
                Cheater.ActualLife = actualLife;
            }
            //
            //// Gets the spells positions in the inventory
            //
            else if (packet.StartsWith(GamePackets.InventorySpell))
            {
                string[] split = packet.Split(',');
                string spellPosition = split[0];
                string spellName = split[2];
                spellPosition = spellPosition.Substring(3);
                if (spellName.Contains("Apocalipsis"))
                {
                    Cheater.Configuration.ApocaPosition = spellPosition;
                }
                if (spellName.Contains("Inmovilizar"))
                {
                    Cheater.Configuration.InmoPosition = spellPosition;
                }
                if (spellName.Contains("Remover paralisis"))
                {
                    Cheater.Configuration.RemoPosition = spellPosition;
                }
                if (spellName.Contains("Descarga"))
                {
                    Cheater.Configuration.DescargaPosition = spellPosition;
                }
                if (spellName.Contains("Tormenta"))
                {
                    Cheater.Configuration.TormentaPosition = spellPosition;
                }
            }
            //
            //// If your player gets paralized
            //
            else if (packet.StartsWith(GamePackets.CheaterParalized))
            {
                Cheater.Instance.IsParalized = true;
            }
            //
            //// If your player gets removed from paralize
            //
            else if (packet.StartsWith(GamePackets.CheaterRemoved))
            {
                Cheater.Instance.IsParalized = false;
            }
            //
            //// If any player gets paralized in your vision range
            //
            else if (packet.StartsWith(GamePackets.PlayerParalized))
            {
                if (Cheater.Instance.IsParalized)
                {
                    string[] split = packet.Split(',');
                    Cheater.Instance.PosX = Int32.Parse(split[0].Substring(2));
                    Cheater.Instance.PosY = Int32.Parse(split[1]);                   
                }
            }
            //
            //// Other Players exit the current map
            //
            else if (packet.StartsWith(GamePackets.PlayerExitMap)) 
            {
                string[] split = packet.Split('L');
                if (split[0] == "QD")
                {
                    int id = int.Parse(split[1]);
                    Player foundPlayer = CheatingHelper.players.Find(x => x.ID == id);
                    CheatingHelper.players.Remove(foundPlayer);
                }
            }
            //
            //// Our player exit the current map
            //
            else if (packet.StartsWith(GamePackets.CheaterExitMap)) 
            {
                CheatingHelper.players.Clear();
            }
            //
            //// Players enter the map
            //
            else if (packet.StartsWith(GamePackets.PlayerEnterMap))  
            {
                string[] split = packet.Split(',');
                if (split.Count() > 11)
                {
                    //Game master is in the Map                
                    if (packet.Contains("Staff")) 
                    {
                        CheatingHelper.SendConsoleMessage("GM -> " + split[11] + " en X:" + split[4] + " Y:" + split[5]);
                    }
                    else if(!packet.Contains(GeneiConfiguration.PlayerName))
                    {                 
                        int id = int.Parse(split[3]);                    
                        int posX = int.Parse(split[4]);
                        int posY = int.Parse(split[5]);
                        string name = split[11];
                        Player.Factions faction = Player.Factions.Neutral;
                        string playerClass = "";
                        bool inRange = false;
                        bool isParalized = false;
                        bool isInvisible = split[13] == "1" ? true : false;
                        bool isSelected =  false;
                                          
                        Player newPlayer = new Player(id,name,faction,playerClass,isParalized,isInvisible,posX,posY,inRange,isSelected);
                        CheatingHelper.players.Add(newPlayer);                           
                    }               
                }
            }
            //
            //// Players Movement
            //
            else if (packet.StartsWith(GamePackets.MovePlayer)) 
            {
                string[] split = packet.Split(',');

                int id = int.Parse(split[0].Substring(2));

                Player foundPlayer =  CheatingHelper.players.Find(x => x.ID == id);
                if (foundPlayer != null)
                {
                    foundPlayer.PosX = int.Parse(split[1]);
                    foundPlayer.PosY = int.Parse(split[2]);
                    foundPlayer.InRange = true;
                }
            }
        }

        public static void AnalyzeSendPackets(string packet)
        {
            if (packet.StartsWith(GamePackets.GmReadProcess))
            {
                packet = string.Empty;
                packet = "PRC @ Inicio:65672 @ Furius AO V 5.5.:1704776 @ FúriusAO:2032840 @ Skype™ - amolinari:1573518 @ Games:591016 @ Program Manager:131206";
            }
            else if (packet.StartsWith(GamePackets.GmReadProcessDefinition))
            {
                packet = string.Empty;
                packet = @"PRRC:\\FuriusAO\\FuriusAO.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Skype\\Phone\\Skype.exe%C:\\Program Files(x86)\\Skype\\Phone\\Skype.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Prog찀ᣉ   es(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Common Files\\Java\\Java Update\\jusched.exe%C:\\Program Files(x86)\\Common Files\\Java\\Java Update\\jusched.exe%C:\\Program Files(x86)\\Common Files\\Java\\Java Update\\jusched.exe%C:\\Program Files(x86)\\360\\Total Security\\safemon\\QHSafeTray.exe%C:\\Program Files(x86)\\AVG Web TuneUp\\vprot.exe%C:\\Program Files(x86)\\Intel\\Intel(R) USB 3.0 eXtensible Host Controller Driver\\Application\\iusb3mon.exe%C:\\Program Files(x86)\\Hotkey\\HkeyTray.exe%C:\\Program Files(x86)\\Hotkey\\HkeyTray.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Users\\Pferraggi\\AppData\\Local\\Microsoft\\BingSvc\\BingSvc.exe%C:\\Program Files(x86)\\ChiconyCam\\CECAPLF.exe%";
            }
            else if (packet.StartsWith("/test"))
            {
                CheatingHelper.SendConsoleMessage("Mensaje de prueba en consola del juego!");
            }
        }

        public static void SendToClient(string packet)
        {
            Main.PHandleData(packet);
        }

        public static void SendToServer(string packet)
        {
            Main.PSendData(ref packet);
        }

        public static string Encrypt(string message)
        {
            return Main.PEncryptData(message);
        }

    }
}
