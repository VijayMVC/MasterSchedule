using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    public class DelayShipmentViewModel
    {
        public string Style { get; set; }
        public string ProductNo { get; set; }
        public int AssemblyBalance { get; set; }
        public int SewingBalance { get; set; }
        public int OutsoleBalance { get; set; }

        public DateTime OrderEFD { get; set; }
        public DateTime OrderCSD { get; set; }
    }
}
