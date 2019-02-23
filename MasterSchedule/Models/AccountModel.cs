using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    public class AccountModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public bool UpperRMSchedule { get; set; }
        public bool OutsoleRMSchedule { get; set; }
        //
        public bool OutsoleMaterialEdit { get; set; }

        public bool UpperComponentRMSchedule { get; set; }
        public bool CartonRMSchedule { get; set; }
        public bool ImportData { get; set; }
        public bool ReviseData { get; set; }
        public bool CutPrepMaster { get; set; }
        public bool SewingMaster { get; set; }
        public bool OutsoleMaster { get; set; }
        public bool SockliningMaster { get; set; }
        public bool AssemblyMaster { get; set; }
        public bool ViewOnly { get; set; }
        public bool OffDay { get; set; }
        public bool ProductionMemo { get; set; }
        public bool Notifications { get; set; }
        public bool Sortable { get; set; }
        public bool Simulation { get; set; }
        public bool OutsoleWH { get; set; }

        public bool Insock { get; set; }
    }
}
