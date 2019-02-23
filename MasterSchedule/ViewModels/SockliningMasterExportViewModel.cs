using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;


namespace MasterSchedule.ViewModels
{
    public class SockliningMasterExportViewModel
    {
        public int Sequence { get; set; }
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string PatternNo { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public string SockliningLine { get; set; }
        public string SockliningMatsArrival { get; set; }
        public bool IsSockliningMatsArrivalOk { get; set; }
        public DateTime SewingStartDate { get; set; }
        public string SewingBalance { get; set; }
        public DateTime OutsoleStartDate { get; set; }
        public string OutsoleBalance { get; set; }
        public DateTime AssemblyStartDate { get; set; }
        public int SockliningQuota { get; set; }
        public DateTime SockliningStartDate { get; set; }
        public DateTime SockliningFinishDate { get; set; }
        public string InsoleBalance { get; set; }
        public string InsockBalance { get; set; }
    }
}
