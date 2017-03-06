using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AORNet.Configurations
{
    public static class LibraryConfiguration
    {
        public const string ServerName = "FuriusAO";
        public const int StructAddress = 0x7354FC;
        public const int OpenForReading = 0x1F0FFF;
        public static bool IsProcessOpen = false;
        public static Process AOProcess;
        public static IntPtr ProcessHandle;
        public static Keys ApocaKey;
        public static Keys DescargaKey;
        public static Keys InmoKey;
        public static Keys TormentaKey;
        public static Keys RemoKey = Keys.Home;

    }
}
