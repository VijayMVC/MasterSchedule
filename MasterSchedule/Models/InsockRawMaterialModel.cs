using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class InsockRawMaterialModel
    {
        public string ProductNo { get; set; }
        public int InsockSupplierId { get; set; }
        public string InsockSupplierName { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ActualDate { get; set; }
    }
}
