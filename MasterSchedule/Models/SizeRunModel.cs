using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class SizeRunModel
    {
        public string ProductNo { get; set; }
        public string OutsoleSize { get; set; }
        public string MidsoleSize { get; set; }
        public string SizeNo { get; set; }
        public int Quantity { get; set; }
        public bool UpdateOutsoleSizeByArticle { get; set; }
    }
}
