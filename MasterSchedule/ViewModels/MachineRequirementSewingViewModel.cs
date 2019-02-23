using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class MachineRequirementSewingViewModel
    {
        public string ArticleNo { get; set; }
        public float SewingWorker { get; set; }
        public float SewingSmallComputer { get; set; }
        public float SewingBigComputer { get; set; }
        public float SewingUltrasonic { get; set; }
        public float SewingFourNeedleFlat { get; set; }
        public float SewingFourNeedlePost { get; set; }
        public float SewingLongTable { get; set; }
        public float SewingEyeleting { get; set; }
        public float SewingZZBinding { get; set; }
        public float SewingHotmeltlMachine { get; set; }
        public float SewingHandHeldHotmelt { get; set; }
        public float SewingStationaryHHHotmelt { get; set; }
    }
}
