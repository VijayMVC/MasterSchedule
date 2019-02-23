using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class AssemblyMasterModel
    {
        public string ProductNo { get; set; }
        public int Sequence { get; set; }
        public string AssemblyLine { get; set; }
        public DateTime AssemblyStartDate { get; set; }
        public DateTime AssemblyFinishDate { get; set; }
        public int AssemblyQuota { get; set; }
        public string AssemblyActualStartDate { get; set; }
        public string AssemblyActualFinishDate { get; set; }
        public string AssemblyBalance { get; set; }

        public bool IsSequenceUpdate { get; set; }
        public bool IsAssemblyLineUpdate { get; set; }
        public bool IsAssemblyStartDateUpdate { get; set; }
        public bool IsAssemblyFinishDateUpdate { get; set; }
        public bool IsAssemblyQuotaUpdate { get; set; }
        public bool IsAssemblyActualStartDateUpdate { get; set; }
        public bool IsAssemblyActualFinishDateUpdate { get; set; }
        public bool IsAssemblyBalanceUpdate { get; set; }
    }
}
