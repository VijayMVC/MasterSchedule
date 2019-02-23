using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;

namespace MasterSchedule.ViewModels
{
    class OutsoleWHInventoryDetailViewModel
    {
        public string OutsoleCode { get; set; }
        public List<String> ProductNoList { get; set; }
        
        public DateTime EFD { get; set; }
        public string ArticleNo { get; set; }
        public List<String> SupplierName { get; set; }

        public List<Int32> MatchingDetail { get; set; }
        public List<Int32> QuantityDetail { get; set; }
        public int MatchingSummary { get; set; }
        public int QuantitySummary { get; set; }
        
    }
}
