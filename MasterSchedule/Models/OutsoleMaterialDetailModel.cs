using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialDetailModel
    {
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public int Reject { get; set; }
        public int QuantityExcess { get; set; }
        //public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string IndexNo { get; set; }
        public int Round { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public string Status { get; set; }

    }
}
