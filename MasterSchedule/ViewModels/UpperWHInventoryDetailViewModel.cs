using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class UpperWHInventoryDetailViewModel
    {
        //public string AssemblyLine { get; set; }
        public string ProductNo { get; set; }
        public string ShoeName { get; set; }
        public string ArticleNo { get; set; }
        public DateTime ETD { get; set; }
        public int Quantity { get; set; }
        public int ReleaseQuantity { get; set; }
        public int SewingOutput { get; set; }
        public int OutsoleOutput { get; set; }
        public int Matching { get; set; }
    }
}
