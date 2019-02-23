using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class MachineRequirementAssemblyViewModel
    {
        public string ArticleNo { get; set; }
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
