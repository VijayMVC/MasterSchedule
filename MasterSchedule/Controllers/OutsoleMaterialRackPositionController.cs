using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MasterSchedule.Models;
using System.Data.SqlClient;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class OutsoleMaterialRackPositionController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

        public static List<OutsoleMaterialRackPositionModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<OutsoleMaterialRackPositionModel>("EXEC spm_SelectOutsoleMaterialRackPosition @ProductNo", @ProductNo).ToList();
        }

        public static bool Insert(OutsoleMaterialRackPositionModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @RackNumber = new SqlParameter("@RackNumber", model.RackNumber);
            var @CartonNumber = new SqlParameter("@CartonNumber", model.CartonNumber);

            if (db.ExecuteStoreCommand("EXEC spm_InsertOutsoleMaterialRackPosition @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber", @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber) >= 1)
            {
                return true;
            }
            return false;
        }

        public static bool Delete(OutsoleMaterialRackPositionModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @OutsoleSupplierId = new SqlParameter("@OutsoleSupplierId", model.OutsoleSupplierId);
            var @RackNumber = new SqlParameter("@RackNumber", model.RackNumber);
            var @CartonNumber = new SqlParameter("@CartonNumber", model.CartonNumber);

            if (db.ExecuteStoreCommand("EXEC spm_DeleteOutsoleMaterialRackPosition @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber", @ProductNo, @OutsoleSupplierId, @RackNumber, @CartonNumber) >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
