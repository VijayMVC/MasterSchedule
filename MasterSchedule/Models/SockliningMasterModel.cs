using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class SockliningMasterModel
    {
        public string ProductNo { get; set; }
        public int Sequence { get; set; }
        public string SockliningLine { get; set; }
        public DateTime SockliningStartDate { get; set; }
        public DateTime SockliningFinishDate { get; set; }
        public int SockliningQuota { get; set; }
        public string SockliningActualStartDate { get; set; }
        public string SockliningActualFinishDate { get; set; }
        public string InsoleBalance { get; set; }
        public string InsockBalance { get; set; }

        public bool IsSequenceUpdate { get; set; }
        public bool IsSockliningLineUpdate { get; set; }
        public bool IsSockliningStartDateUpdate { get; set; }
        public bool IsSockliningFinishDateUpdate { get; set; }
        public bool IsSockliningQuotaUpdate { get; set; }
        public bool IsSockliningActualStartDateUpdate { get; set; }
        public bool IsSockliningActualFinishDateUpdate { get; set; }
        public bool IsInsoleBalanceUpdate { get; set; }
        public bool IsInsockBalanceUpdate { get; set; }    
    }
}
