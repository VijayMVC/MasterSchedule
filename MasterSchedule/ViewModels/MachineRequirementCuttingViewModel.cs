using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class MachineRequirementCuttingViewModel
    {
        public string ArticleNo { get; set; }
        public float CuttingQuantity { get; set; }
        public float CuttingWorker { get; set; }
        public float CuttingArmClicker { get; set; }
        public float CuttingBeam { get; set; }
        public float CuttingCutStrap { get; set; }
        public float CuttingLaser { get; set; }
        public float CuttingSkiving { get; set; }
    }
}
