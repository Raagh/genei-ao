using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using EasyHook;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var argentumProcess = Process.Start("D:\\FuriusAO\\FuriusAO.exe");
            try
            {
                RemoteHooking.IpcCreateServer<RemoteService>(ref Defaults.CHANNEL_NAME, WellKnownObjectMode.Singleton);
                int processID = -1;
                foreach (Process p in Process.GetProcessesByName(Defaults.PROCESS_NAME))
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
                    Defaults.CURRENT_DIR + Defaults.DLL_NAME, Defaults.CURRENT_DIR + Defaults.DLL_NAME,
                    new Object[] {Defaults.CHANNEL_NAME});
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
