using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media;
using EasyHook;
using GalaSoft.MvvmLight.CommandWpf;
using GeneiAO.Model;

namespace GeneiAO.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region -- Instance --

        public MainViewModel()
        {
            HookCommand = new RelayCommand(async () => await Hook());
            _instance = this;
        }

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    return _instance = new MainViewModel();
                }
                return _instance;
            }
        }

        #endregion

        #region -- Private Properties --
        private bool _status;
        private string _error;
        private List<Player> _players;
        #endregion

        #region -- Public Properties --

        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged();
            }
        }
        public List<Player> Players
        {
            get { return _players; }
            set
            {
                _players = value;
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
                RemoteHooking.IpcCreateServer<RemoteService>(ref Defaults.CHANNEL_NAME, WellKnownObjectMode.Singleton);
                int processID = -1;
                foreach (Process p in Process.GetProcessesByName(Defaults.PROCESS_NAME))
                {
                    processID = p.Id;
                    break;
                }
                if (processID == -1)
                {
                    //Error = "No process exists with that name!";
                    return;
                }
                RemoteHooking.Inject(processID, InjectionOptions.DoNotRequireStrongName, Defaults.CURRENT_DIR + Defaults.DLL_NAME , Defaults.CURRENT_DIR + Defaults.DLL_NAME, new Object[] { Defaults.CHANNEL_NAME });
            }
            catch (Exception ExtInfo)
            {
               //Error = "There was an error while connecting to target:\r\n" + ExtInfo.ToString();
            }
        }

        #endregion

    }
}
