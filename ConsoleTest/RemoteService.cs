using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace ConsoleTest
{
    public class RemoteService : MarshalByRefObject
    {
        public void IsInstalled(bool isInstalled)
        {          
            Console.WriteLine("Hooked installed succesfully.");
        }

        public void Receive(string message)
        {
            Console.WriteLine(message);
        }

        public void ErrorHandler(Exception dllException)
        {
            Console.WriteLine("An error ocurred in the dll client." + dllException);
        }

    }
}
