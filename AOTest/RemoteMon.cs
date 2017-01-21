using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOTest
{
    public class RemoteMon : MarshalByRefObject
    {
        public void IsInstalled(int InClientPID)
        {
            Console.WriteLine("Successfully injected into PID {0}.", InClientPID);
        }

        public void Receive(string message)
        {
            Console.WriteLine(message);
        }

        public void ErrorHandler(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

    }
}
