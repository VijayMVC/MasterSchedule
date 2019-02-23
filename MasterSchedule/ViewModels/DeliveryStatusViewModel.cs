using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    public class DeliveryStatusViewModel
    {
        // i have nothing to share about job in my company. everything i do is check some issues computer, mouse, keyboard, network etc
        // additional replace ink for printer, bring something to storage
        // just code a little bit. around 3 --> 6 projects
        // follow and repair computers in factory. if it has a problem, i need to go there and check
        // read information on the internet
        // everything i can receive when i work at saoviet is

        public string ProductNo { get; set; }
        public string Country { get; set; }
        public string ArticleNo { get; set; }
        public string ShoeName { get; set; }
        public int Quantity { get; set; }
        public DateTime ETD { get; set; }
        public string Supplier { get; set; }
        public DateTime SupplierETD { get; set; }
        public string Actual { get; set; }
        public bool IsFinished { get; set; }
    }
}
