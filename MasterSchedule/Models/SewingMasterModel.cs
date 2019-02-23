using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class SewingMasterModel
    {
        public string ProductNo { get; set; }
        public int Sequence { get; set; }
        public string SewingLine { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime SewingFinishDate { get; set; }
        public int SewingQuota { get; set; }
        //
        public string SewingPrep { get; set; }

        public string SewingActualStartDate { get; set; }
        public string SewingActualFinishDate { get; set; }

        public string SewingActualStartDateAuto { get; set; }
        public string SewingActualFinishDateAuto { get; set; }

        public string SewingBalance { get; set; }
        public DateTime CutAStartDate { get; set; }
        public DateTime CutAFinishDate { get; set; }
        public int CutAQuota { get; set; }
        public string CutAActualStartDate { get; set; }
        public string CutAActualFinishDate { get; set; }
        public string CutABalance { get; set; }
        public string PrintingBalance { get; set; }
        public string H_FBalance { get; set; }
        public string EmbroideryBalance { get; set; }

        public string CutBActualStartDate { get; set; }
        public string CutBBalance { get; set; }
        public string AutoCut { get; set; }

        //
        public string LaserCut { get; set; }
        public string HuasenCut { get; set; }

        public bool IsSequenceUpdate { get; set; }
        public bool IsSewingLineUpdate { get; set; }
        public bool IsSewingStartDateUpdate { get; set; }
        public bool IsSewingFinishDateUpdate { get; set; }

        public bool IsSewingQuotaUpdate { get; set; }
        public bool IsSewingPrepUpdate { get; set; }

        public bool IsSewingActualStartDateUpdate { get; set; }
        public bool IsSewingActualFinishDateUpdate { get; set; }

        public bool IsSewingActualStartDateAutoUpdate { get; set; }
        public bool IsSewingActualFinishDateAutoUpdate { get; set; }

        public bool IsSewingBalanceUpdate { get; set; }
        public bool IsCutAStartDateUpdate { get; set; }
        public bool IsCutAFinishDateUpdate { get; set; }
        public bool IsCutAQuotaUpdate { get; set; }
        public bool IsCutAActualStartDateUpdate { get; set; }
        public bool IsCutAActualFinishDateUpdate { get; set; }
        public bool IsCutABalanceUpdate { get; set; }
        public bool IsPrintingBalanceUpdate { get; set; }
        public bool IsH_FBalanceUpdate { get; set; }
        public bool IsEmbroideryBalanceUpdate { get; set; }
        public bool IsCutBBalanceUpdate { get; set; }
        public bool IsCutBActualStartDateUpdate { get; set; }
        public bool IsAutoCutUpdate { get; set; }
        public bool IsLaserCutUpdate { get; set; }
        public bool IsHuasenCutUpdate { get; set; }
    }
}
