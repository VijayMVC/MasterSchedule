using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MasterSchedule.ViewModels
{
    public class ETDFilterViewModel : INotifyPropertyChanged
    {
        private bool? _IsSelected;
        public bool? IsSelected
        {
            get { return _IsSelected; }
            set
            {
                _IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                OnPropertyChanged("Date");
            }
        }

        private string _Content;
        public string Content
        {
            get { return _Content; }
            set
            {
                _Content = value;
                OnPropertyChanged("Content");
            }
        }

        private bool _IsRoot;
        public bool IsRoot
        {
            get { return _IsRoot; }
            set
            {
                _IsRoot = value;
                OnPropertyChanged("IsRoot");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
