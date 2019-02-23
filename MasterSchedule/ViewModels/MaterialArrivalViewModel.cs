using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
namespace MasterSchedule.ViewModels
{
    public class MaterialArrivalViewModel
    {
        public DateTime Date { get; set; }
        public Brush Foreground { get; set; }
        public Brush Background { get; set; }
        public bool IsMaterialArrivalOk { get; set; }
    }
}
