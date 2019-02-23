using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    class CompletionStatusViewModel
    {
        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ShoeName { get; set; }
        public DateTime ETD { get; set; }
        public string LoadingDate { get; set; }
        public string ArticleNo { get; set; }
        public int Quantity { get; set; }
        public string CutAFinishDate { get; set; }
        public string SewingLine { get; set; }
        public string SewingFinishDate { get; set; }
        public string OutsoleLine { get; set; }
        public string OutsoleFinishDate { get; set; }
        public string AssemblyLine { get; set; }
        public string AssemblyFinishDate { get; set; }
        public bool IsFinished { get; set; }
    }
}
