using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
namespace MasterSchedule.ViewModels
{
    class OutsoleRawMaterialViewModel
    {
        public string ProductNo { get; set; }
        public OutsoleSuppliersModel Supplier { get; set; }
        public string ETD { get; set; }
        public DateTime ETDReal { get; set; }
    }
}
