using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using EasyHook;
using GalaSoft.MvvmLight.CommandWpf;
using GeneiAO.Model;

namespace GeneiAO.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string ChannelName = null;

        private string currdir = "D:\\Development\\GitHub\\genei-ao\\AORNet\\bin\\Debug" + "\\";

        public MainViewModel()
        {
            HookCommand = new RelayCommand(async () => await Hook());
        }

        private string error;

        public string ErrorMessage
        {
            get { return error; }
            set
            {
                error = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand HookCommand { get; }
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        async Task Hook()
        {
            try
            {
                RemoteHooking.IpcCreateServer<RemoteService>(ref ChannelName, WellKnownObjectMode.Singleton);
                int processid = -1;

                string w = "FuriusAO";

                foreach (Process p in Process.GetProcessesByName(w))
                {
                    processid = p.Id;
                    break;
                }
                if (processid == -1)
                {
                    ErrorMessage = "No process exists with that name!";
                    return;
                }
                RemoteHooking.Inject(processid, InjectionOptions.DoNotRequireStrongName, currdir + "AORNet.dll",currdir + "AORNet.dll", new Object[] { ChannelName });
                ErrorMessage = "Dll injected succesfully!";
            }
            catch (Exception ExtInfo)
            {
                ErrorMessage = "There was an error while connecting to target:\r\n" + ExtInfo.ToString();
            }

            //while (true)
            //{
            //    Thread.Sleep(1000);
            //}
        }

    }
}
