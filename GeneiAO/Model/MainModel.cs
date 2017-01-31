using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GeneiAO.Model
{
    public class MainModel : MarshalByRefObject,INotifyPropertyChanged
    {
        #region -- Instance --
        private static MainModel _instance;

        public static MainModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    return _instance = new MainModel();
                }
                return _instance;
            }
        }

        #endregion

        #region -- Private Properties --
        private bool _status;
        private string _error;
        #endregion

        #region -- Public Properties --
        public string LatestPropertyExecuted;

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

        #endregion

        #region -- Events --

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            LatestPropertyExecuted = name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

    }
}
