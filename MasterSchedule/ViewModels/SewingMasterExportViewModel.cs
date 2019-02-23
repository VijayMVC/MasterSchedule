using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;


namespace MasterSchedule.ViewModels
{
    public class SewingMasterExportViewModel
    {
        public int Sequence { get; set; }
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string PatternNo { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public string SewingLine { get; set; }
        public string UpperMatsArrival { get; set; }
        public bool IsUpperMatsArrivalOk { get; set; }
        public DateTime CutAStartDate { get; set; }
        public string SewingMatsArrival { get; set; }
        public bool IsSewingMatsArrivalOk { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public string OSMatsArrival { get; set; }
        public bool IsOSMatsArrivalOk { get; set; }
        public string OSBalance { get; set; }
        public int SewingQuota { get; set; }
        public string SewingBalance { get; set; }
        public string CutABalance { get; set; }
        public string MemoId { get; set; }


        public object CutBBalance { get; set; }
    }
}
