using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class MachineRequirementModel
    {
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public string PM { get; set; }

        // Cutting  
        public string CuttingQuantity { get; set; }
        public string CuttingWorker { get; set; }
        public string CuttingArmClicker { get; set; }
        public string CuttingBeam { get; set; }
        public string CuttingCutStrap { get; set; }
        public string CuttingLaser { get; set; }
        public string CuttingPuncherHole { get; set; }
        public string CuttingSkiving { get; set; }


        // Prep

        //public float PrepQuantity { get; set; }
        public string PrepWorker { get; set; }
        public string PrepVerticalHF { get; set; }
        public string PrepHorizontalHF { get; set; }
        public string PrepOnlineHeatPress { get; set; }
        public string PrepAutoHF { get; set; }
        public string PrepInye { get; set; }
        public string PrepHotmeltMachine { get; set; }



        // Sewing

        public string SewingQuantity { get; set; }
        public string SewingWorker { get; set; }
        public string SewingSmallComputer { get; set; }
        public string SewingBigComputer { get; set; }
        public string SewingUltrasonic { get; set; }
        public string Sewing4NeedleFlat { get; set; }
        public string Sewing4NeedlePost { get; set; }
        public string SewingLongTable { get; set; }
        public string SewingEyeleting { get; set; }
        public string SewingZZBinding { get; set; }
        public string SewingHotmeltMachine { get; set; }
        public string SewingHandHeldHotmelt { get; set; }
        public string SewingStationaryHHHotmelt { get; set; }

        // Stockfit
        public string StockfitQuantity { get; set; }
        public string StockfitWorker { get; set; }
        public string StockfitVerticalBuffing { get; set; }
        public string StockfitHorizontalBuffing { get; set; }
        public string StockfitSideBuffing { get; set; }
        public string StockfitOutsoleStitching { get; set; }
        public string StockfitAutoBuffing { get; set; }
        public string StockfitHydraulicCutting { get; set; }
        public string StockfitPadPrinting { get; set; }
    

        // Assembly
        public string AssemblyQuantity { get; set; }
        public string AssemblyWorker { get; set; }
        public string AssemblyToeLasting { get; set; }
        public string AssemblySideLasting { get; set; }
        public string AssemblyHeelLasting { get; set; }
        public string AssemblySidePress { get; set; }
        public string AssemblyTopDown { get; set; }
        public string AssemblyHotmeltMachine { get; set; }
        public string AssemblySocklinerHotmelt { get; set; }
        public string AssemblyVWrinkleRemover { get; set; }
   
    }
}
