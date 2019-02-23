using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class UpperWHInventoryViewModel
    {
        public string AssemblyLine { get; set; }
        public List<String> ProductNoList { get; set; }
        public int SewingOutput { get; set; }
        public int OutsoleOutput { get; set; }
        public int Matching { get; set; }
    }
}
