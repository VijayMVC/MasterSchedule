using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;


namespace MasterSchedule.ViewModels
{
    public class CutprepMasterExportViewModel
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
        public DateTime SewingStartDate { get; set; }
        public int SewingQuota { get; set; }
        public string SewingBalance { get; set; }
        public DateTime CutAStartDate { get; set; }
        public DateTime CutAFinishDate { get; set; }
        public int CutAQuota { get; set; }
        public string AutoCut { get; set; }
        public string LaserCut { get; set; }
        public string HuasenCut { get; set; }
        public string CutABalance { get; set; }
        public string PrintingBalance { get; set; }
        public string H_FBalance { get; set; }
        public string EmbroideryBalance { get; set; }
        public string CutBBalance { get; set; }
        public string MemoId { get; set; }
        
    }
}
