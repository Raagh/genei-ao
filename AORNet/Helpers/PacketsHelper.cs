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
        public static string AnalyzeHandlePackets(string packet)
        {
            //
            ////Checks if there is any Chest in the map
            //
            if (packet.StartsWith(GamePackets.MapItem))
            {
                string[] split = packet.Split(new Char[] { ',' });
                string itemId = split[0];
                itemId = itemId.Substring(2);

                if (itemId.Contains(GamePackets.InventoryItemTreasureId))
                {
                    CheatingHelper.SendConsoleMessage("Cofre encontrado en x:" + split[1] + " y: " + split[2]);
                }
            }

            //
            ////Gets the spells positions
            //
            if (packet.StartsWith(GamePackets.InventorySpell))
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
            ////If you Get Inmovilizado
            //
            if (packet.StartsWith(GamePackets.CheaterParalized))
            {
                Cheater.Instance.IsParalized = true;
            }

            //
            ////If you Get Removed from inmo spell
            //
            if (packet.StartsWith(GamePackets.CheaterRemoved))
            {
                Cheater.Instance.IsParalized = false;
            }

            //
            //// If the player Gets paralized, Cast Remover Paralisis
            //
            if (packet.StartsWith(GamePackets.PlayerParalized))
            {
                if (Cheater.Instance.IsParalized)
                {
                    string[] split = packet.Split(new Char[] { ',' });
                    int posX = int.Parse(split[0].Substring(2));
                    int posY = int.Parse(split[1]);
                    Cheater.Instance.PosX = int.Parse(split[0].Substring(2));
                    Cheater.Instance.PosY = int.Parse(split[1]);

                    CheatingHelper.CastSpell(Cheater.Configuration.RemoPosition,posX, posY);

                    Cheater.Instance.IsParalized = false;
                }
            }

            //
            ////Players exits the map
            //
            if (packet.StartsWith(GamePackets.PlayerExitMap)) 
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
            ////Cheater exits the map
            //
            if (packet.StartsWith(GamePackets.CheaterExitMap)) 
            {
                //charactersListInAutoAim.Clear();
                //charactersListInMap.Clear();
                ////monstersListInMap.Clear();
                //selectedCharacter = 0;
                //selectedName = string.Empty;
            }

            return packet;
        }

        public static string AnalyzeSendPackets(string packet)
        {
            if (packet.StartsWith(GamePackets.GmReadProcess))
            {
                packet = string.Empty;
                packet = "PRC @ Inicio:65672 @ Furius AO V 5.5.:1704776 @ FúriusAO:2032840 @ Skype™ - amolinari:1573518 @ Games:591016 @ Program Manager:131206";
            }

            if (packet.Contains(GamePackets.GmReadProcessDefinition))
            {
                packet = string.Empty;
                packet = @"PRRC:\\FuriusAO\\FuriusAO.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Skype\\Phone\\Skype.exe%C:\\Program Files(x86)\\Skype\\Phone\\Skype.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Prog찀ᣉ   es(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Program Files(x86)\\Common Files\\Java\\Java Update\\jusched.exe%C:\\Program Files(x86)\\Common Files\\Java\\Java Update\\jusched.exe%C:\\Program Files(x86)\\Common Files\\Java\\Java Update\\jusched.exe%C:\\Program Files(x86)\\360\\Total Security\\safemon\\QHSafeTray.exe%C:\\Program Files(x86)\\AVG Web TuneUp\\vprot.exe%C:\\Program Files(x86)\\Intel\\Intel(R) USB 3.0 eXtensible Host Controller Driver\\Application\\iusb3mon.exe%C:\\Program Files(x86)\\Hotkey\\HkeyTray.exe%C:\\Program Files(x86)\\Hotkey\\HkeyTray.exe%C:\\Program Files(x86)\\Google\\Chrome\\Application\\chrome.exe%C:\\Users\\Pferraggi\\AppData\\Local\\Microsoft\\BingSvc\\BingSvc.exe%C:\\Program Files(x86)\\ChiconyCam\\CECAPLF.exe%";
            }

            return packet;
        }
    }
}
