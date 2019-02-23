using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class UpperComponentWHInventoryViewModel
    {
        public string OutsoleCode { get; set; }
        public List<String> ProductNoList { get; set; }
        public string ProductNo { get; set; }
        public int Quantity { get; set; }
        public int Matching { get; set; }
        public string UpperComponentName { get; set; }
        //public List<String> SupplierNameList { get; set; }
        public List<Int32> UpperComponentIDList { get; set; }
        //public int FinishedOutsoleQuantity { get; set; }
    }
}
