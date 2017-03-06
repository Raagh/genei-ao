using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AORNet.Model;

namespace AORNet.Helpers
{
    public static class PacketsHelper
    {
        public static void AnalyzeHandlePackets(string packet)
        {    
            //
            //// Checks if there is any Chest in the map
            //
            if (packet.StartsWith(GamePackets.MapItem))
            {
                string[] split = packet.Split(new Char[] { ',' });
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
                string itemName = split[1];
                if (itemName.Contains("roja"))
                {
                    Cheater.Configuration.RedPotionPosition = inventoryPosition;
                }
                else if (itemName.Contains("azul"))
                {
                    Cheater.Configuration.BluePotionPosition = inventoryPosition;
                }
                else if (itemName.Contains("amarilla"))
                {
                    Cheater.Configuration.YellowPotionPosition = inventoryPosition;
                }
            }
            //
            //// Gets the spells positions in the inventory
            //
            else if (packet.StartsWith(GamePackets.InventorySpell))
            {
                string[] split = packet.Split(new Char[] { ',' });
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
                    string[] split = packet.Split(new Char[] { ',' });
                    Cheater.Instance.PosX = Int32.Parse(split[0].Substring(2));
                    Cheater.Instance.PosY = Int32.Parse(split[1]);                   
                }
            }
            //
            //// Other Players exit the current map
            //
            else if (packet.StartsWith(GamePackets.PlayerExitMap)) 
            {
                string[] split = packet.Split(new Char[] { 'L' });
                if (split[0] == "QD")
                {
                    //Character foundPlayerInAutoAim = charactersListInAutoAim.Find(x => x.id == int.Parse(split[1]));
                    //Character foundPlayerInMap = charactersListInMap.Find(x => x.id == int.Parse(split[1]));
                    ////Monster foundMonster = monstersListInMap.Find(x => x.ID == int.Parse(split[1]));
                    //charactersListInAutoAim.Remove(foundPlayerInAutoAim);
                    //charactersListInMap.Remove(foundPlayerInMap);
                    ////monstersListInMap.Remove(foundMonster);
                }
            }
            //
            //// Our player exit the current map
            //
            else if (packet.StartsWith(GamePackets.CheaterExitMap)) 
            {
                //charactersListInAutoAim.Clear();
                //charactersListInMap.Clear();
                ////monstersListInMap.Clear();
                //selectedCharacter = 0;
                //selectedName = string.Empty;
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
