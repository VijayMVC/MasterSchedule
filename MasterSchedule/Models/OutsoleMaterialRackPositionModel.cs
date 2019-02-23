using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMaterialRackPositionModel
    {
        public string ProductNo { get; set; }
        public int OutsoleSupplierId { get; set; }
        public string RackNumber { get; set; }
        public int CartonNumber { get; set; }
    }
}
