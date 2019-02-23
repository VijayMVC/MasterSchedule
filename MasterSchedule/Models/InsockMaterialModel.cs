using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class InsockMaterialModel
    {
        public string ProductNo { get; set; }
        public int InsockSupplierId { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public int QuantityReject { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime ModifiedTimeReject { get; set; }
    }
}
