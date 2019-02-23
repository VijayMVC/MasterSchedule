using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;


namespace MasterSchedule.ViewModels
{
    public class RawMaterialViewModel : INotifyPropertyChanged
    {
        public string ProductNo { get; set; }
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
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string PatternNo { get; set; }
        public string OutsoleCode { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }

        public DateTime CutAStartDate { get; set; }
        public Brush CutAStartDateForeground { get; set; }

        public DateTime AssyStartDate { get; set; }
        public Brush AssyStartDateForeground { get; set; }

        public string MemoId { get; set; }

        public string LAMINATION_ETD { get; set; }
        public string LAMINATION_ActualDate { get; set; }
        public string LAMINATION_Remarks { get; set; }

        public string TAIWAN_ETD { get; set; }
        public string TAIWAN_ActualDate { get; set; }
        public string TAIWAN_Remarks { get; set; }
        
        public string CUTTING_ETD { get; set; }
        public string CUTTING_ActualDate { get; set; }
        public string CUTTING_Remarks { get; set; }

        public string LEATHER_ETD { get; set; }
        public string LEATHER_ActualDate { get; set; }
        public string LEATHER_Remarks { get; set; }

        public string SEMIPROCESS_ETD { get; set; }
        public string SEMIPROCESS_ActualDate { get; set; }
        public string SEMIPROCESS_Remarks { get; set; }

        public string SEWING_ETD { get; set; }
        public string SEWING_ActualDate { get; set; }
        // addition code
        public DateTime Sewing_StartDate { get; set; }
        //
        public string SEWING_Remarks { get; set; }


        public string _OUTSOLE_ETD;
        public string OUTSOLE_ETD
        {
            get { return _OUTSOLE_ETD; }
            set
            {
                _OUTSOLE_ETD = value;
                OnPropertyChanged("OUTSOLE_ETD");
            }
        }
        public string _OUTSOLE_ActualDate;
        public string OUTSOLE_ActualDate
        {
            get { return _OUTSOLE_ActualDate; }
            set
            {
                _OUTSOLE_ActualDate = value;
                OnPropertyChanged("OUTSOLE_ActualDate");
            }
        }
        public string _OUTSOLE_Remarks;
        public string OUTSOLE_Remarks
        {
            get { return _OUTSOLE_Remarks; }
            set
            {
                _OUTSOLE_Remarks = value;
                OnPropertyChanged("OUTSOLE_Remarks");
            }
        }
        public SolidColorBrush _OUTSOLE_ActualDate_BACKGROUND;
        public SolidColorBrush OUTSOLE_ActualDate_BACKGROUND 
        {
            get { return _OUTSOLE_ActualDate_BACKGROUND; }
            set
            {
                _OUTSOLE_ActualDate_BACKGROUND = value;
                OnPropertyChanged("OUTSOLE_ActualDate_BACKGROUND");
            }
        }
        public SolidColorBrush OUTSOLE_ActualDate_FOREGROUND { get; set; }

        public string _INSOCK_ETD;
        public string INSOCK_ETD
        {
            get { return _INSOCK_ETD; }
            set
            {
                _INSOCK_ETD = value;
                OnPropertyChanged("INSOCK_ETD");
            }
        }

        public string _INSOCK_ActualDate;
        public string INSOCK_ActualDate
        {
            get { return _INSOCK_ActualDate; }
            set
            {
                _INSOCK_ActualDate = value;
                OnPropertyChanged("INSOCK_ActualDate");
            }
        }

        public string _INSOCK_Remarks;
        public string INSOCK_Remarks
        {
            get { return _INSOCK_Remarks; }
            set
            {
                _INSOCK_Remarks = value;
                OnPropertyChanged("INSOCK_Remarks");
            }
        }

        public string _UPPERCOMPONENT_ETD;
        public string UPPERCOMPONENT_ETD
        {
            get { return _UPPERCOMPONENT_ETD; }
            set
            {
                _UPPERCOMPONENT_ETD = value;
                OnPropertyChanged("UPPERCOMPONENT_ETD");
            }
        }
        public string _UPPERCOMPONENT_ActualDate;
        public string UPPERCOMPONENT_ActualDate
        {
            get { return _UPPERCOMPONENT_ActualDate; }
            set
            {
                _UPPERCOMPONENT_ActualDate = value;
                OnPropertyChanged("UPPERCOMPONENT_ActualDate");
            }
        }
        public string _UPPERCOMPONENT_Remarks;
        public string UPPERCOMPONENT_Remarks
        {
            get { return _UPPERCOMPONENT_Remarks; }
            set
            {
                _UPPERCOMPONENT_Remarks = value;
                OnPropertyChanged("UPPERCOMPONENT_Remarks");
            }
        }

        public string SECURITYLABEL_ETD { get; set; }
        public string SECURITYLABEL_ActualDate { get; set; }
        public string SECURITYLABEL_Remarks { get; set; }

        public string ASSEMBLY_ETD { get; set; }
        public string ASSEMBLY_ActualDate { get; set; }
        public string ASSEMBLY_Remarks { get; set; }

        public string SOCKLINING_ETD { get; set; }
        public string SOCKLINING_ActualDate { get; set; }
        public string SOCKLINING_Remarks { get; set; }

        public string CARTON_ETD { get; set; }
        public string CARTON_ActualDate { get; set; }
        public string CARTON_Remarks { get; set; }
        public Brush CARTON_ActualDate_Background { get; set; }
        public Brush CARTON_ETD_Background { get; set; }

        public DateTime CARTON_ETD_Sort { get; set; }
        public DateTime CARTON_ActualDate_Sort { get; set; }

        public string LoadingDate { get; set; }

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
