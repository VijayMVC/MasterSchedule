using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class RawMaterialModel
    {
        public string ProductNo { get; set; }
        public int MaterialTypeId { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ActualDate { get; set; }
        public string Remarks { get; set; }
        public DateTime ModifiedTime { get; set; }

        public bool IsETDUpdate { get; set; }
        public bool IsActualDateUpdate { get; set; }
        public bool IsRemarksUpdate { get; set; }
    }
}
