using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class OutsoleSuppliersController
    {
        public static List<OutsoleSuppliersModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<OutsoleSuppliersModel>("EXEC spm_SelectOutsoleSuppliers").ToList();
        }
    }
}
