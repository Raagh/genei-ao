using System;
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

        MainModel model = new MainModel();

        public void IsInstalled(int InClientPID)
        {
            model.Message = "Client " + InClientPID + "Hooked Succesfully";
            //MessageBox.Show("Client " + InClientPID + " - Hooked Succesfully");
        }

        public void Receive(string message)
        {

        }

        public void ErrorHandler(Exception ex)
        {

        }

    }
}
