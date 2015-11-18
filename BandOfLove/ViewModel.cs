using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BandOfLove
{
    public class ViewModel : INotifyPropertyChanged
    {
        public static ViewModel instance;

        // INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public ObservableCollection<SweetNothing> SweetNothings { get; set; }

        private string _Sender { get; set; }
        public string Sender
        {
            get
            {
                return _Sender;
            }
            set
            {
                _Sender = value;
                NotifyPropertyChanged("Sender");
            }
        }

        private bool _IsEditMode;
        public bool IsEditMode
        {
            get
            {
                return _IsEditMode;
            }
            set
            {
                _IsEditMode = value;
                NotifyPropertyChanged("IsEditMode");
            }
        }

        private SweetNothing _SelectedMessage;
        public SweetNothing SelectedMessage
        {
            get
            {
                if (_SelectedMessage == null)
                {
                    _SelectedMessage = SweetNothings.FirstOrDefault();
                }
                return _SelectedMessage;
            }
            set
            {
                if (value != this._SelectedMessage)
                {
                    this._SelectedMessage = value;
                    NotifyPropertyChanged("SelectedMessage");
                }
            }
        }

        public ViewModel()
        {
            instance = this;

            this.Sender = "From Corbin";

            this.IsEditMode = false;

            SweetNothings = new ObservableCollection<SweetNothing>() {
                new SweetNothing() { Message = "I love you! :)" },
                new SweetNothing() { Message = "You're so cute! :D" },
                new SweetNothing() { Message = "I miss youuu!" },
                new SweetNothing() { Message = "<3 <3 <3" },
                new SweetNothing() { Message = "HUGGG!!!" },
                new SweetNothing() { Message = "Poke!" }
            };
        }
    }
}
