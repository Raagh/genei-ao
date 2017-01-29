using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GeneiAO.Model
{
    public class MainModel : INotifyPropertyChanged
    {

        private static MainModel _instance;
        private string _message;
        public event PropertyChangedEventHandler PropertyChanged;
        public string LatestProperty;
      
        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            LatestProperty = name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
    }
}
