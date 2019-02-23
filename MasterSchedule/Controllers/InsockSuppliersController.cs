using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class InsockSuppliersController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
        public static List<InsockSuppliersModel> Select()
        {
            return db.ExecuteStoreQuery<InsockSuppliersModel>("EXEC spm_SelectInsockSuppliers").ToList();
        }
    }
}
