using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterSchedule.Models;

namespace MasterSchedule.ViewModels
{
    public class UpperComponentRawMaterialViewModel
    {
        public string ProductNo { get; set; }
        public UpperComponentModel UpperComponents { get; set; }
        public string ETD { get; set; }
        public DateTime ETDReal { get; set; }
    }
}
