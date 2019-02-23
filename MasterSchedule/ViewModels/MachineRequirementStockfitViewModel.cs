using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class MachineRequirementStockfitViewModel
    {
        public string ArticleNo { get; set; }
        public float StockfitQuantity { get; set; }
        public float StockfitWorker { get; set; }
        public float StockfitVerticalBuffing { get; set; }
        public float StockfitHorizontalBuffing { get; set; }
        public float StockfitSideBuffing { get; set; }
        public float StockfitOutsoleStitching { get; set; }
        public float StockfitAutoBuffing { get; set; }
        public float StockfitHydraulicCutting { get; set; }
        public float StockfitPadPrinting { get; set; }
    }
}
