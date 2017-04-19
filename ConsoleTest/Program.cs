using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using EasyHook;

namespace ConsoleTest
{
    class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            var argentumProcess = Process.Start("D:\\FuriusAO\\FuriusAO.exe");
            try
            {
                RemoteHooking.IpcCreateServer<RemoteService>(ref Defaults.ChannelName, WellKnownObjectMode.Singleton);
                int processID = -1;
                foreach (Process p in Process.GetProcessesByName(Defaults.ProcessName))
                {
                    processID = p.Id;
                    break;
                }
                if (processID == -1)
                {
                    Console.WriteLine("No process exists with that name!");
                    return;
                }
                RemoteHooking.Inject(processID, InjectionOptions.DoNotRequireStrongName,
                    Defaults.CurrentDir + Defaults.DllName, Defaults.CurrentDir + Defaults.DllName,
                    new Object[] {Defaults.ChannelName});

                var handle = GetConsoleWindow();

                // Hide
                //ShowWindow(handle, SW_HIDE);

                Console.ReadKey();
            }
            catch (Exception ExtInfo)
            {
                Console.WriteLine("There was an error while connecting to target:\r\n" + ExtInfo.ToString());
            }
            finally
            {
                argentumProcess.Close();
            }          
        }

    }
}
