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

        public string Message
        {
            get { return MainModel.Instance.Message; }
            set
            {
                MainModel.Instance.Message = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HookCommand = new RelayCommand(async () => await Hook());
            MainModel.Instance.PropertyChanged += (s, e) =>{ OnPropertyChanged(MainModel.Instance.LatestProperty);};
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private async Task Hook()
        {
            try
            {
                RemoteHooking.IpcCreateServer<RemoteService>(ref Defaults.ChannelName, WellKnownObjectMode.Singleton);
                int processID = -1;
                foreach (Process p in Process.GetProcessesByName("FuriusAO"))
                {
                    processID = p.Id;
                    break;
                }
                if (processID == -1)
                {
                    Message = "No process exists with that name!";
                    return;
                }
                RemoteHooking.Inject(processID, InjectionOptions.DoNotRequireStrongName, Defaults.CurrentDir + "AORNet.dll",Defaults.CurrentDir + "AORNet.dll", new Object[] { Defaults.ChannelName });
            }
            catch (Exception ExtInfo)
            {
                Message = "There was an error while connecting to target:\r\n" + ExtInfo.ToString();
            }
        }

    }
}
