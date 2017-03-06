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
