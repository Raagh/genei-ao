using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GeneiAO.ViewModel;

namespace GeneiAO.Model
{
    public class RemoteService : MarshalByRefObject
    {
        public void IsInstalled(int inClientPid)
        {
            MainModel.Instance.Message = "Client " + inClientPid + " - Hooked Succesfully";
        }

        public void Receive(string message)
        {
            
        }

        public void ErrorHandler(Exception dllException)
        {
            throw dllException;
        }

    }
}
