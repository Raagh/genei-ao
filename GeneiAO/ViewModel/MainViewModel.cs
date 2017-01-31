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
        public MainViewModel()
        {
            HookCommand = new RelayCommand(async () => await Hook());
            MainModel.Instance.PropertyChanged += (s, e) => { OnPropertyChanged(MainModel.Instance.LatestPropertyExecuted); };
        }

        #region -- Public Properties --

        public string Error
        {
            get { return MainModel.Instance.Error; }
            set
            {
                MainModel.Instance.Error = value;
                OnPropertyChanged();
            }
        }

        public bool Status
        {
            get { return MainModel.Instance.Status; }
            set
            {
                MainModel.Instance.Status = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region -- Events --
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region -- Commands --

        public RelayCommand HookCommand { get; }

        private async Task Hook()
        {
            try
            {
                RemoteHooking.IpcCreateServer<MainModel>(ref Defaults.CHANNEL_NAME, WellKnownObjectMode.Singleton);
                int processID = -1;
                foreach (Process p in Process.GetProcessesByName(Defaults.PROCESS_NAME))
                {
                    processID = p.Id;
                    break;
                }
                if (processID == -1)
                {
                    Error = "No process exists with that name!";
                    return;
                }
                RemoteHooking.Inject(processID, InjectionOptions.DoNotRequireStrongName, Defaults.CURRENT_DIR + "AORNet.dll", Defaults.CURRENT_DIR + "AORNet.dll", new Object[] { Defaults.CHANNEL_NAME });
            }
            catch (Exception ExtInfo)
            {
                Error = "There was an error while connecting to target:\r\n" + ExtInfo.ToString();
            }
        }

        #endregion



    }
}
