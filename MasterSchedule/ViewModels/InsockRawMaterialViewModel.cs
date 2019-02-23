using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;

namespace MasterSchedule.ViewModels
{
    public class InsockRawMaterialViewModel
    {
        public string ProductNo { get; set; }
        public InsockSuppliersModel InsockSupplier { get; set; }
        public String ETD { get; set; }
        public DateTime ETDReal { get; set; }
    }
    
}
