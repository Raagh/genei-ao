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
        public RelayCommand HookCommand { get; }
        public MainModel MainModel = new MainModel();
        private string ChannelName = null;
        private string currdir = "D:\\Development\\GitHub\\genei-ao\\AORNet\\bin\\Debug" + "\\";
        public string Message
        {
            get { return MainModel.Message; }
            set
            {
                MainModel.Message = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HookCommand = new RelayCommand(async () => await Hook());
            MainModel.PropertyChanged += (s, e) =>{ OnPropertyChanged();};
        }

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
                    //Message = "No process exists with that name!";
                    return;
                }
                RemoteHooking.Inject(processid, InjectionOptions.DoNotRequireStrongName, currdir + "AORNet.dll",currdir + "AORNet.dll", new Object[] { ChannelName });
                //Message = "Dll injected succesfully!";
            }
            catch (Exception ExtInfo)
            {
                //Message = "There was an error while connecting to target:\r\n" + ExtInfo.ToString();
            }

            //while (true)
            //{
            //    Thread.Sleep(1000);
            //}
        }

    }
}
