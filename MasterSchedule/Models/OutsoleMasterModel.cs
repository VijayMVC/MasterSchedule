using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class OutsoleMasterModel
    {
        public string ProductNo { get; set; }
        public int Sequence { get; set; }
        public string OutsoleLine { get; set; }
        public DateTime OutsoleStartDate { get; set; }
        public DateTime OutsoleFinishDate { get; set; }
        public int OutsoleQuota { get; set; }
        public string OutsoleActualStartDate { get; set; }
        public string OutsoleActualFinishDate { get; set; }
        public string OutsoleActualStartDateAuto { get; set; }
        public string OutsoleActualFinishDateAuto { get; set; }
        public string OutsoleBalance { get; set; }

        public bool IsSequenceUpdate { get; set; }
        public bool IsOutsoleLineUpdate { get; set; }
        public bool IsOutsoleStartDateUpdate { get; set; }
        public bool IsOutsoleFinishDateUpdate { get; set; }
        public bool IsOutsoleQuotaUpdate { get; set; }
        public bool IsOutsoleActualStartDateUpdate { get; set; }
        public bool IsOutsoleActualFinishDateUpdate { get; set; }
        public bool IsOutsoleActualStartDateAutoUpdate { get; set; }
        public bool IsOutsoleActualFinishDateAutoUpdate { get; set; }
        public bool IsOutsoleBalanceUpdate { get; set; }
    }
}
