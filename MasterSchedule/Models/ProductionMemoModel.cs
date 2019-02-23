using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    class ProductionMemoModel
    {
        public string MemoId { get; set; }
        public string SectionId { get; set; }
        public string ProductionNumbers { get; set; }
        public byte[] Picture { get; set; }
        public byte[] Picture1 { get; set; }
        public byte[] Picture2 { get; set; }
        public byte[] Picture3 { get; set; }
        public byte[] Picture4 { get; set; }
    }
}
