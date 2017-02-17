using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AORNet.Helpers
{
    public static class MemoryHelper
    {
        //Open Process
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        //Read Memory
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

        //Write Memory
        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesWritten);


        public static bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }

        public static int Read(IntPtr handle, int address)
        {
            //1. Prepare buffer and pointer
            byte[] dataBuffer = new byte[4];
            IntPtr bytesRead = IntPtr.Zero;

            //2. Read
            ReadProcessMemory(handle, (IntPtr)address, dataBuffer, dataBuffer.Length, out bytesRead);

            //3. Convert the content of your buffer to int
            return BitConverter.ToInt32(dataBuffer, 0);
        }

        public static bool Write(IntPtr handle, int address, int value)
        {
            //1. Create buffer and pointer
            byte[] dataBuffer = BitConverter.GetBytes(value);
            IntPtr bytesWritten = IntPtr.Zero;

            //2. Write
            WriteProcessMemory(handle, (IntPtr)address, dataBuffer, dataBuffer.Length, out bytesWritten);

            //3. Error handling
            if (bytesWritten == IntPtr.Zero)
            {
                //Console.WriteLine("No se escribio Nada!");
                return false;
            }
            if (bytesWritten.ToInt32() < dataBuffer.Length)
            {
                //Console.WriteLine("Escribimos {0} de un total de {1} bytes!", bytesWritten.ToInt32(), dataBuffer.Length.ToString());
                return false;
            }
            return true;
        }
    }
}
