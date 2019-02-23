using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using MasterSchedule.Entities;
namespace MasterSchedule.Controllers
{
    class MaterialTypeController
    {
        public static List<MaterialTypeModel> Select()
        {
            SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
            return db.ExecuteStoreQuery<MaterialTypeModel>("EXEC spm_SelectMaterialType").ToList();
        }
    }
}
