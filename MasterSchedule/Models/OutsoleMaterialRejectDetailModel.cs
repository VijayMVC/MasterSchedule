using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialRejectDetailModel
    {
        public string ProductNo { get; set; }
        public string IndexNo { get; set; }
        public int Round { get; set; }
        public int OutsoleSupplierId { get; set; }
        public int OutsoleMaterialRejectIssuesId { get; set; }
        public string SizeNo { get; set; }
        public int QuantityReject { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
