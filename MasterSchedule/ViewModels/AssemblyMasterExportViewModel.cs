using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;


namespace MasterSchedule.ViewModels
{
    public class AssemblyMasterExportViewModel
    {
        public int Sequence { get; set; }
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string LastCode { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public string AssemblyLine { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public string OutsoleMatsArrival { get; set; }
        public bool IsOutsoleMatsArrivalOk { get; set; }
        public string AssemblyMatsArrival { get; set; }
        public bool IsAssemblyMatsArrivalOk { get; set; }
        public string CartonMatsArrival { get; set; }
        public DateTime AssemblyStartDate { get; set; }
        public DateTime AssemblyFinishDate { get; set; }
        public int AssemblyQuota { get; set; }
        public string SewingBalance { get; set; }
        public string OutsoleBalance { get; set; }
        public string InsoleBalance { get; set; }
        public string InsockBalance { get; set; }
        public string AssemblyBalance { get; set; }
        public string MemoId { get; set; }
    }
}
