using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class MachineRequirementPrepViewModel
    {
        public string ArticleNo { get; set; }
        public float PrepWorker { get; set; }
        public float PrepVerticalHF { get; set; }
        public float PrepHorizontalHF { get; set; }
        public float PrepAutoHF { get; set; }
        public float PrepInye { get; set; }
        public float PrepHotmeltMachine { get; set; }
    }
}
