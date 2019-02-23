using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;


namespace MasterSchedule.ViewModels
{
    public class SewingMasterViewModel : INotifyPropertyChanged
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
            get { return _ProductNo;}
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
            get { return _ShoeName;}
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

        private string _UpperMatsArrival;
        public string UpperMatsArrival
        {
            get { return _UpperMatsArrival; }
            set
            {
                _UpperMatsArrival = value;
                OnPropertyChanged("UpperMatsArrival");
            }
        }

        private DateTime _UpperMatsArrivalOrginal;
        public DateTime UpperMatsArrivalOrginal
        {
            get { return _UpperMatsArrivalOrginal; }
            set
            {
                _UpperMatsArrivalOrginal = value;
                OnPropertyChanged("UpperMatsArrivalOrginal");
            }
        }

        private Brush _UpperMatsArrivalForeground;
        public Brush UpperMatsArrivalForeground
        {
            get { return _UpperMatsArrivalForeground; }
            set
            {
                _UpperMatsArrivalForeground = value;
                OnPropertyChanged("UpperMatsArrivalForeground");
            }
        }

        private Brush _UpperMatsArrivalBackground;
        public Brush UpperMatsArrivalBackground
        {
            get { return _UpperMatsArrivalBackground; }
            set
            {
                _UpperMatsArrivalBackground = value;
                OnPropertyChanged("UpperMatsArrivalBackground");
            }
        }  

        private string _SewingMatsArrival; 
        public string SewingMatsArrival
        {
            get { return _SewingMatsArrival; }
            set
            {
                _SewingMatsArrival = value;
                OnPropertyChanged("SewingMatsArrival");
            }
        }

        private DateTime _SewingMatsArrivalOrginal;
        public DateTime SewingMatsArrivalOrginal
        {
            get { return _SewingMatsArrivalOrginal; }
            set
            {
                _SewingMatsArrivalOrginal = value;
                OnPropertyChanged("SewingMatsArrivalOrginal");
            }
        }

        private Brush _SewingMatsArrivalForeground;
        public Brush SewingMatsArrivalForeground
        {
            get { return _SewingMatsArrivalForeground; }
            set
            {
                _SewingMatsArrivalForeground = value;
                OnPropertyChanged("SewingMatsArrivalForeground");
            }
        }

        private Brush _SewingMatsArrivalBackground;
        public Brush SewingMatsArrivalBackground
        {
            get { return _SewingMatsArrivalBackground; }
            set
            {
                _SewingMatsArrivalBackground = value;
                OnPropertyChanged("SewingMatsArrivalBackground");
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

        private Brush _SewingStartDateForeground;//Auto
        public Brush SewingStartDateForeground
        {
            get { return _SewingStartDateForeground; }
            set
            {
                _SewingStartDateForeground = value;
                OnPropertyChanged("SewingStartDateForeground");
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

        private Brush _SewingFinishDateForeground;//Auto
        public Brush SewingFinishDateForeground
        {
            get { return _SewingFinishDateForeground; }
            set
            {
                _SewingFinishDateForeground = value;
                OnPropertyChanged("SewingFinishDateForeground");
            }
        }

        private DateTime _OSFinishDate; //Get From Outsole Schedule
        public DateTime OSFinishDate
        {
            get { return _OSFinishDate; }
            set
            {
                _OSFinishDate = value;
                OnPropertyChanged("OSFinishDate");
            }
        }        

        private string _OSBalance; //Get From Outsole Schedule
        public string OSBalance
        {
            get { return _OSBalance; }
            set
            {
                _OSBalance = value;
                OnPropertyChanged("OSBalance");
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

        private string _SewingPrep; //Input Manual
        public string SewingPrep
        {
            get { return _SewingPrep; }
            set 
            {
                _SewingPrep = value;
                OnPropertyChanged("SewingPrep");
            }
        }

        private string _SewingActualStartDate; //Input Manual
        public string SewingActualStartDate
        {
            get { return _SewingActualStartDate; }
            set
            {
                _SewingActualStartDate = value;
                OnPropertyChanged("SewingActualStartDate");
            }
        }

        private string _SewingActualFinishDate; //Input Manual
        public string SewingActualFinishDate
        {
            get { return _SewingActualFinishDate; }
            set
            {
                _SewingActualFinishDate = value;
                OnPropertyChanged("SewingActualFinishDate");
            }
        }

        private string _SewingActualStartDateAuto; //Auto base on SewingBalance
        public string SewingActualStartDateAuto
        {
            get { return _SewingActualStartDateAuto; }
            set
            {
                _SewingActualStartDateAuto = value;
                OnPropertyChanged("SewingActualStartDateAuto");
            }
        }

        private string _SewingActualFinishDateAuto; //Auto base on SewingBalance
        public string SewingActualFinishDateAuto
        {
            get { return _SewingActualFinishDateAuto; }
            set
            {
                _SewingActualFinishDateAuto = value;
                OnPropertyChanged("SewingActualFinishDateAuto");
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

        private DateTime _CutAStartDate; //Auto
        public DateTime CutAStartDate
        {
            get { return _CutAStartDate; }
            set
            {
                _CutAStartDate = value;
                OnPropertyChanged("CutAStartDate");
            }
        }

        private Brush _CutAStartDateForeground;//Auto
        public Brush CutAStartDateForeground
        {
            get { return _CutAStartDateForeground; }
            set
            {
                _CutAStartDateForeground = value;
                OnPropertyChanged("CutAStartDateForeground");
            }
        }

        private DateTime _CutAFinishDate; //Auto
        public DateTime CutAFinishDate
        {
            get { return _CutAFinishDate; }
            set
            {
                _CutAFinishDate = value;
                OnPropertyChanged("CutAFinishDate");
            }
        }

        private int _CutAQuota;//Input Manual
        public int CutAQuota
        {
            get { return _CutAQuota; }
            set
            {
                _CutAQuota = value;
                OnPropertyChanged("CutAQuota");
            }
        }

        private string _CutAActualStartDate;//Input Manual 
        public string CutAActualStartDate
        {
            get { return _CutAActualStartDate; }
            set
            {
                _CutAActualStartDate = value;
                OnPropertyChanged("CutAActualStartDate");
            }
        }

        private string _CutAActualFinishDate;//Input Manual
        public string CutAActualFinishDate
        {
            get { return _CutAActualFinishDate; }
            set
            {
                _CutAActualFinishDate = value;
                OnPropertyChanged("CutAActualFinishDate");
            }
        }

        private string _CutABalance;//Input Manual
        public string CutABalance
        {
            get { return _CutABalance; }
            set
            {
                _CutABalance = value;
                OnPropertyChanged("CutABalance");
            }
        }

        private string _PrintingBalance;//Input Manual
        public string PrintingBalance
        {
            get { return _PrintingBalance; }
            set
            {
                _PrintingBalance = value;
                OnPropertyChanged("PrintingBalance");
            }
        }

        private string _H_FBalance;//Input Manual
        public string H_FBalance
        {
            get { return _H_FBalance; }
            set
            {
                _H_FBalance = value;
                OnPropertyChanged("H_FBalance");
            }
        }

        private string _EmbroideryBalance; //Input Manual
        public string EmbroideryBalance
        {
            get { return _EmbroideryBalance; }
            set
            {
                _EmbroideryBalance = value;
                OnPropertyChanged("EmbroideryBalance");
            }
        }


        private string _CutBActualStartDate;//Input Manual 
        public string CutBActualStartDate
        {
            get { return _CutBActualStartDate; }
            set
            {
                _CutBActualStartDate = value;
                OnPropertyChanged("CutBActualStartDate");
            }
        }

        private string _CutBBalance; //Input Manual
        public string CutBBalance
        {
            get { return _CutBBalance; }
            set
            {
                _CutBBalance = value;
                OnPropertyChanged("CutBBalance");
            }
        }

        private string _AutoCut; //Input Manual
        public string AutoCut
        {
            get { return _AutoCut; }
            set
            {
                _AutoCut = value;
                OnPropertyChanged("AutoCut");
            }
        }

        private string _LaserCut; // Input Manual
        public string LaserCut 
        {
            get { return _LaserCut; }
            set
            {
                _LaserCut = value;
                OnPropertyChanged("LaserCut");
            }
        }

        // im on the way to be a .netdeveloper. 
        private string _HuasenCut; // Input Manual
        public string HuasenCut
        {
            get { return _HuasenCut; }
            set
            {
                _HuasenCut = value;
                OnPropertyChanged("HuasenCut");
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
