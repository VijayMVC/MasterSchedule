using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;


namespace MasterSchedule.ViewModels
{
    public class OutsoleMasterExportViewModel
    {
        public int Sequence { get; set; }
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public string OutsoleCode { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public string OutsoleLine { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public string OutsoleMatsArrival { get; set; }
        public bool IsOutsoleMatsArrivalOk { get; set; }
        public string OutsoleWHBalance { get; set; }
        public DateTime OutsoleStartDate { get; set; }
        public DateTime OutsoleFinishDate { get; set; }
        public int SewingQuota { get; set; }
        public int OutsoleQuota { get; set; }
        public string SewingBalance { get; set; }
        public string OutsoleBalance { get; set; }
        public string ReleasedQuantity { get; set; }
        public string MemoId { get; set; }
    }
}
