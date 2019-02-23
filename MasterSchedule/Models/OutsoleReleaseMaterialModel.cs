using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleReleaseMaterialModel
    {
        public string ReportId { get; set; }
        public string ProductNo { get; set; }
        public int Cycle { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
