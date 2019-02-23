using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class UpperComponentController
    {
        public static List<UpperComponentModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<UpperComponentModel>("EXEC spm_SelectUpperComponents").ToList();
        }
    }
}
