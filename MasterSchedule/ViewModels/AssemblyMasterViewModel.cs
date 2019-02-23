using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;

namespace MasterSchedule.ViewModels
{
    public class AssemblyMasterViewModel : INotifyPropertyChanged
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

        private string _LastCode;
        public string LastCode
        {
            get { return _LastCode; }
            set
            {
                _LastCode = value;
                OnPropertyChanged("LastCode");
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

        private int _SewingQuota; //Auto
        public int SewingQuota
        {
            get { return _SewingQuota; }
            set
            {
                _SewingQuota = value;
                OnPropertyChanged("SewingQuota");
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

        private string _AssemblyMatsArrival;
        public string AssemblyMatsArrival
        {
            get { return _AssemblyMatsArrival; }
            set
            {
                _AssemblyMatsArrival = value;
                OnPropertyChanged("AssemblyMatsArrival");
            }
        }

        private Brush _AssemblyMatsArrivalForeground;
        public Brush AssemblyMatsArrivalForeground
        {
            get { return _AssemblyMatsArrivalForeground; }
            set
            {
                _AssemblyMatsArrivalForeground = value;
                OnPropertyChanged("AssemblyMatsArrivalForeground");
            }
        }

        private Brush _AssemblyMatsArrivalBackground;
        public Brush AssemblyMatsArrivalBackground
        {
            get { return _AssemblyMatsArrivalBackground; }
            set
            {
                _AssemblyMatsArrivalBackground = value;
                OnPropertyChanged("AssemblyMatsArrivalBackground");
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


        private string _CartonMatsArrival;
        public string CartonMatsArrival
        {
            get { return _CartonMatsArrival; }
            set
            {
                _CartonMatsArrival = value;
                OnPropertyChanged("CartonMatsArrival");
            }
        }

        private Brush _CartonMatsArrivalForeground;
        public Brush CartonMatsArrivalForeground
        {
            get { return _CartonMatsArrivalForeground; }
            set
            {
                _CartonMatsArrivalForeground = value;
                OnPropertyChanged("CartonMatsArrivalForeground");
            }
        }

        private Brush _CartonMatsArrivalBackground;
        public Brush CartonMatsArrivalBackground
        {
            get { return _CartonMatsArrivalBackground; }
            set
            {
                _CartonMatsArrivalBackground = value;
                OnPropertyChanged("CartonMatsArrivalBackground");
            }
        }

        private string _AssemblyLine;
        public string AssemblyLine
        {
            get { return _AssemblyLine; }
            set
            {
                _AssemblyLine = value;
                OnPropertyChanged("AssemblyLine");
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

        private Brush _AssemblyStartDateForeground;//Auto
        public Brush AssemblyStartDateForeground
        {
            get { return _AssemblyStartDateForeground; }
            set
            {
                _AssemblyStartDateForeground = value;
                OnPropertyChanged("AssemblyStartDateForeground");
            }
        }

        private DateTime _AssemblyFinishDate; //Auto
        public DateTime AssemblyFinishDate
        {
            get { return _AssemblyFinishDate; }
            set
            {
                _AssemblyFinishDate = value;
                OnPropertyChanged("AssemblyFinishDate");
            }
        }

        private Brush _AssemblyFinishDateForeground; //Auto
        public Brush AssemblyFinishDateForeground
        {
            get { return _AssemblyFinishDateForeground; }
            set
            {
                _AssemblyFinishDateForeground = value;
                OnPropertyChanged("AssemblyFinishDateForeground");
            }
        }

        private int _AssemblyQuota;//Input Manual
        public int AssemblyQuota
        {
            get { return _AssemblyQuota; }
            set
            {
                _AssemblyQuota = value;
                OnPropertyChanged("AssemblyQuota");
            }
        }

        private string _AssemblyActualStartDate; //Input Manual
        public string AssemblyActualStartDate
        {
            get { return _AssemblyActualStartDate; }
            set
            {
                _AssemblyActualStartDate = value;
                OnPropertyChanged("AssemblyActualStartDate");
            }
        }

        private string _AssemblyActualFinishDate; //Input Manual
        public string AssemblyActualFinishDate
        {
            get { return _AssemblyActualFinishDate; }
            set
            {
                _AssemblyActualFinishDate = value;
                OnPropertyChanged("AssemblyActualFinishDate");
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

        private string _SockliningBalance; //Input Manual -> SizeRun
        public string SockliningBalance
        {
            get { return _SockliningBalance; }
            set
            {
                _SockliningBalance = value;
                OnPropertyChanged("SockliningBalance");
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

        private string _OutsoleReleasedQuantity; // Only view from outsole
        public string OutsoleReleasedQuantity
        {
            get { return _OutsoleReleasedQuantity; }
            set
            {
                _OutsoleReleasedQuantity = value;
                OnPropertyChanged("OutosleReleasedQuantity");
            }
        }
    }
}
