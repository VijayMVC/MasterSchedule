using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleRawMaterialModel
    {
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ActualDate { get; set; }
    }
}
