using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.ViewModels
{
    public class NoticeOutsoleWHInventoryViewModel
    {
        public string Style { get; set; }
        public string ProductNo { get; set; }
        public int QuantityDelivery { get; set; }
        public int QuantityNotDelivery { get; set; }
        public string Supplier { get; set; }
        //public List<string> SupplierList { get; set; }
        public string OutsoleCode { get; set; }
        public int QuantityReject { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime DeliveryEFDDate { get; set; }
        public DateTime SewingStartDate { get; set; }
        public DateTime OrderEFD { get; set; }
        public DateTime OrderCSD { get; set; }
    }
}
