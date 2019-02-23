using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterSchedule.Models
{
    class SectionModel
    {
        public static List<SectionModel> CreateList()
        {
            List<SectionModel> sectionList = new List<SectionModel>();
            sectionList.Add(new SectionModel() { SectionId = "WH", Name = "Warehoure", });
            sectionList.Add(new SectionModel() { SectionId = "SEW", Name = "Sewing", });
            sectionList.Add(new SectionModel() { SectionId = "CP", Name = "Cut-prep", });
            sectionList.Add(new SectionModel() { SectionId = "OS", Name = "Outsole", });
            sectionList.Add(new SectionModel() { SectionId = "ASSY", Name = "Assembly", });

            return sectionList;
        }

        public string SectionId { get; set; }
        public string Name { get; set; }
    }
}
