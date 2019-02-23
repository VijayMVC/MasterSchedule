using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace MasterSchedule.ViewModels
{
    public class SockliningMasterViewModel : INotifyPropertyChanged
    {
        private int _Sequence;
        public int Sequence
        {
            get { return _Sequence; }
            set
            {
                _Sequence = value;
                OnPropertyChanged("Sequence");
            }
        }

        private string _ProductNo;
        public string ProductNo
        {
            get { return _ProductNo; }
            set
            {
                _ProductNo = value;
                OnPropertyChanged("ProductNo");
            }
        }

        private Brush _ProductNoBackground;
        public Brush ProductNoBackground
        {
            get { return _ProductNoBackground; }
            set
            {
                _ProductNoBackground = value;
                OnPropertyChanged("ProductNoBackground");
            }
        }  

        private string _Country;
        public string Country
        {
            get { return _Country; }
            set
            {
                _Country = value;
                OnPropertyChanged("Country");
            }
        }

        private string _ShoeName;
        public string ShoeName
        {
            get { return _ShoeName; }
            set
            {
                _ShoeName = value;
                OnPropertyChanged("ShoeName");
            }
        }

        private string _ArticleNo;
        public string ArticleNo
        {
            get { return _ArticleNo; }
            set
            {
                _ArticleNo = value;
                OnPropertyChanged("ArticleNo");
            }
        }

        private string _PatternNo;
        public string PatternNo
        {
            get { return _PatternNo; }
            set
            {
                _PatternNo = value;
                OnPropertyChanged("PatternNo");
            }
        }

        private int _Quantity;
        public int Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        private DateTime _ETD;
        public DateTime ETD
        {
            get { return _ETD; }
            set
            {
                _ETD = value;
                OnPropertyChanged("ETD");
            }
        }        

        private DateTime _SewingStartDate;//Auto
        public DateTime SewingStartDate
        {
            get { return _SewingStartDate; }
            set
            {
                _SewingStartDate = value;
                OnPropertyChanged("SewingStartDate");
            }
        }        

        private string _SewingBalance; //Input Manual -> SizeRun
        public string SewingBalance
        {
            get { return _SewingBalance; }
            set
            {
                _SewingBalance = value;
                OnPropertyChanged("SewingBalance");
            }
        }

        private DateTime _OutsoleStartDate;//Auto
        public DateTime OutsoleStartDate
        {
            get { return _OutsoleStartDate; }
            set
            {
                _OutsoleStartDate = value;
                OnPropertyChanged("OutsoleStartDate");
            }
        }

        private string _OutsoleBalance; //Input Manual -> SizeRun
        public string OutsoleBalance
        {
            get { return _OutsoleBalance; }
            set
            {
                _OutsoleBalance = value;
                OnPropertyChanged("OutsoleBalance");
            }
        }

        private DateTime _AssemblyStartDate;//Auto
        public DateTime AssemblyStartDate
        {
            get { return _AssemblyStartDate; }
            set
            {
                _AssemblyStartDate = value;
                OnPropertyChanged("AssemblyStartDate");
            }
        }

        private string _SockliningMatsArrival;
        public string SockliningMatsArrival
        {
            get { return _SockliningMatsArrival; }
            set
            {
                _SockliningMatsArrival = value;
                OnPropertyChanged("SockliningMatsArrival");
            }
        }

        private DateTime _SockliningMatsArrivalOrginal;
        public DateTime SockliningMatsArrivalOrginal
        {
            get { return _SockliningMatsArrivalOrginal; }
            set
            {
                _SockliningMatsArrivalOrginal = value;
                OnPropertyChanged("SockliningMatsArrivalOrginal");
            }
        }

        private Brush _SockliningMatsArrivalForeground;
        public Brush SockliningMatsArrivalForeground
        {
            get { return _SockliningMatsArrivalForeground; }
            set
            {
                _SockliningMatsArrivalForeground = value;
                OnPropertyChanged("SockliningMatsArrivalForeground");
            }
        }

        private Brush _SockliningMatsArrivalBackground;
        public Brush SockliningMatsArrivalBackground
        {
            get { return _SockliningMatsArrivalBackground; }
            set
            {
                _SockliningMatsArrivalBackground = value;
                OnPropertyChanged("SockliningMatsArrivalBackground");
            }
        }

        private string _SockliningLine;
        public string SockliningLine
        {
            get { return _SockliningLine; }
            set
            {
                _SockliningLine = value;
                OnPropertyChanged("SockliningLine");
            }
        }

        private DateTime _SockliningStartDate; //Auto
        public DateTime SockliningStartDate
        {
            get { return _SockliningStartDate; }
            set
            {
                _SockliningStartDate = value;
                OnPropertyChanged("SockliningStartDate");
            }
        }

        private Brush _SockliningStartDateForeground; //Auto
        public Brush SockliningStartDateForeground
        {
            get { return _SockliningStartDateForeground; }
            set
            {
                _SockliningStartDateForeground = value;
                OnPropertyChanged("SockliningStartDateForeground");
            }
        }  

        private DateTime _SockliningFinishDate; //Auto
        public DateTime SockliningFinishDate
        {
            get { return _SockliningFinishDate; }
            set
            {
                _SockliningFinishDate = value;
                OnPropertyChanged("SockliningFinishDate");
            }
        }

        private Brush _SockliningFinishDateForeground; //Auto
        public Brush SockliningFinishDateForeground
        {
            get { return _SockliningFinishDateForeground; }
            set
            {
                _SockliningFinishDateForeground = value;
                OnPropertyChanged("SockliningFinishDateForeground");
            }
        }

        private int _SockliningQuota;//Input Manual
        public int SockliningQuota
        {
            get { return _SockliningQuota; }
            set
            {
                _SockliningQuota = value;
                OnPropertyChanged("SockliningQuota");
            }
        }

        private string _SockliningActualStartDate; //Input Manual
        public string SockliningActualStartDate
        {
            get { return _SockliningActualStartDate; }
            set
            {
                _SockliningActualStartDate = value;
                OnPropertyChanged("SockliningActualStartDate");
            }
        }

        private string _SockliningActualFinishDate; //Input Manual
        public string SockliningActualFinishDate
        {
            get { return _SockliningActualFinishDate; }
            set
            {
                _SockliningActualFinishDate = value;
                OnPropertyChanged("SockliningActualFinishDate");
            }
        }

        private string _InsoleBalance;
        public string InsoleBalance
        {
            get { return _InsoleBalance; }
            set
            {
                _InsoleBalance = value;
                OnPropertyChanged("InsoleBalance");
            }
        }

        private string _InsockBalance;
        public string InsockBalance
        {
            get { return _InsockBalance; }
            set
            {
                _InsockBalance = value;
                OnPropertyChanged("InsockBalance");
            }
        }

        private string _AssemblyBalance; //Input Manual -> SizeRun
        public string AssemblyBalance
        {
            get { return _AssemblyBalance; }
            set
            {
                _AssemblyBalance = value;
                OnPropertyChanged("AssemblyBalance");
            }
        }

        private string _SewingLine; //Manual
        public string SewingLine
        {
            get { return _SewingLine; }
            set
            {
                _SewingLine = value;
                OnPropertyChanged("SewingLine");
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
