using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class UpperComponentRawMaterialModel
    {
        public string ProductNo { get; set; }
        public int UpperComponentID { get; set; }
        public string UpperComponentName { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ActualDate { get; set; }
    }
}
