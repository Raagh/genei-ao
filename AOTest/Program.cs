using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyHook;

namespace AOTest
{
    class Program
    {

        public static string ChannelName = null;

        public static string currdir = "D:\\Development\\GitHub\\genei-ao\\AORNet\\bin\\Debug"+ "\\";

        static void Main(string[] args)
        {
            try
            {
                RemoteHooking.IpcCreateServer<RemoteMon>(ref ChannelName, WellKnownObjectMode.Singleton);
                int processid = -1;

                string w = "FuriusAO";

                foreach (Process p in Process.GetProcessesByName(w))
                {
                    processid = p.Id;
                    break;
                }
                if (processid == -1)
                {
                    Console.WriteLine("No process exists with that name!");
                    Console.ReadLine();
                    return;
                }
                RemoteHooking.Inject(processid, InjectionOptions.DoNotRequireStrongName, currdir + "AORNet.dll",
                    currdir + "AORNet.dll", new Object[] {ChannelName});
            }
            catch (Exception ExtInfo)
            {
                Console.WriteLine("There was an error while connecting to target:\r\n{0}", ExtInfo.ToString());
                Console.ReadLine();
            }

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
        
    }
}
