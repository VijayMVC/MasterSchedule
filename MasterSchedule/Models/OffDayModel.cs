using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MasterSchedule.Models
{
    public class OffDayModel : INotifyPropertyChanged
    {
        private int _OffDayId;
        public int OffDayId
        {
            get { return _OffDayId; }
            set
            {
                _OffDayId = value;
                OnPropertyChanged("OffDayId");
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

        private string _Remarks;
        public string Remarks
        {
            get { return _Remarks; }
            set
            {
                _Remarks = value;
                OnPropertyChanged("Remarks");
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
