using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    public class OutsoleDeliveryStatusViewModel
    {
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string OutsoleCode { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public string Supplier { get; set; }
        public DateTime SupplierETD { get; set; }
        public string Actual { get; set; }
        public string ActualQuantity { get; set; }
        public bool IsFinished { get; set; }
        public DateTime SewingStartDate { get; set; }
    }
}
