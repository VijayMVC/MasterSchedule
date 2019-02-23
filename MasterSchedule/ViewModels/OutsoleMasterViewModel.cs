using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace MasterSchedule.ViewModels
{
    public class OutsoleMasterViewModel : INotifyPropertyChanged
    {
        public string MemoId { get; set; }

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

        private string _OutsoleCode;
        public string OutsoleCode
        {
            get { return _OutsoleCode; }
            set
            {
                _OutsoleCode = value;
                OnPropertyChanged("OutsoleCode");
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

        private DateTime _SewingFinishDate; //Auto
        public DateTime SewingFinishDate
        {
            get { return _SewingFinishDate; }
            set
            {
                _SewingFinishDate = value;
                OnPropertyChanged("SewingFinishDate");
            }
        }

        private int _SewingQuota;//Input Manual
        public int SewingQuota
        {
            get { return _SewingQuota; }
            set
            {
                _SewingQuota = value;
                OnPropertyChanged("SewingQuota");
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

        private string _OSMatsArrival;
        public string OSMatsArrival
        {
            get { return _OSMatsArrival; }
            set
            {
                _OSMatsArrival = value;
                OnPropertyChanged("OSMatsArrival");
            }
        }

        private DateTime _OSMatsArrivalOrginal;
        public DateTime OSMatsArrivalOrginal
        {
            get { return _OSMatsArrivalOrginal; }
            set
            {
                _OSMatsArrivalOrginal = value;
                OnPropertyChanged("OSMatsArrivalOrginal");
            }
        }

        private Brush _OSMatsArrivalForeground;
        public Brush OSMatsArrivalForeground
        {
            get { return _OSMatsArrivalForeground; }
            set
            {
                _OSMatsArrivalForeground = value;
                OnPropertyChanged("OSMatsArrivalForeground");
            }
        }

        private Brush _OSMatsArrivalBackground;
        public Brush OSMatsArrivalBackground
        {
            get { return _OSMatsArrivalBackground; }
            set
            {
                _OSMatsArrivalBackground = value;
                OnPropertyChanged("OSMatsArrivalBackground");
            }
        }        

        private string _OutsoleWHBalance;
        public string OutsoleWHBalance
        {
            get { return _OutsoleWHBalance; }
            set
            {
                _OutsoleWHBalance = value;
                OnPropertyChanged("OutsoleWHBalance");
            }
        }

        private string _OutsoleLine;
        public string OutsoleLine
        {
            get { return _OutsoleLine; }
            set
            {
                _OutsoleLine = value;
                OnPropertyChanged("OutsoleLine");
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

        private Brush _OutsoleStartDateForeground;//Auto
        public Brush OutsoleStartDateForeground
        {
            get { return _OutsoleStartDateForeground; }
            set
            {
                _OutsoleStartDateForeground = value;
                OnPropertyChanged("OutsoleStartDateForeground");
            }
        }

        private DateTime _OutsoleFinishDate; //Auto
        public DateTime OutsoleFinishDate
        {
            get { return _OutsoleFinishDate; }
            set
            {
                _OutsoleFinishDate = value;
                OnPropertyChanged("OutsoleFinishDate");
            }
        }

        private Brush _OutsoleFinishDateForeground; //Auto
        public Brush OutsoleFinishDateForeground
        {
            get { return _OutsoleFinishDateForeground; }
            set
            {
                _OutsoleFinishDateForeground = value;
                OnPropertyChanged("OutsoleFinishDateForeground");
            }
        }        

        private int _OutsoleQuota;//Input Manual
        public int OutsoleQuota
        {
            get { return _OutsoleQuota; }
            set
            {
                _OutsoleQuota = value;
                OnPropertyChanged("OutsoleQuota");
            }
        }

        private string _OutsoleActualStartDate; //Input Manual
        public string OutsoleActualStartDate
        {
            get { return _OutsoleActualStartDate; }
            set
            {
                _OutsoleActualStartDate = value;
                OnPropertyChanged("OutsoleActualStartDate");
            }
        }        

        private string _OutsoleActualFinishDate; //Input Manual
        public string OutsoleActualFinishDate
        {
            get { return _OutsoleActualFinishDate; }
            set
            {
                _OutsoleActualFinishDate = value;
                OnPropertyChanged("OutsoleActualFinishDate");
            }
        }

        private string _OutsoleActualStartDateAuto; //Auto base on balance
        public string OutsoleActualStartDateAuto
        {
            get { return _OutsoleActualStartDateAuto; }
            set
            {
                _OutsoleActualStartDateAuto = value;
                OnPropertyChanged("OutsoleActualStartDateAuto");
            }
        }

        private string _OutsoleActualFinishDateAuto; //Auto base on balance
        public string OutsoleActualFinishDateAuto
        {
            get { return _OutsoleActualFinishDateAuto; }
            set
            {
                _OutsoleActualFinishDateAuto = value;
                OnPropertyChanged("OutsoleActualFinishDateAuto");
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

        private string _ReleasedQuantity; //Input Manual -> SizeRun
        public string ReleasedQuantity
        {
            get { return _ReleasedQuantity; }
            set
            {
                _ReleasedQuantity = value;
                OnPropertyChanged("ReleasedQuantity");
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
