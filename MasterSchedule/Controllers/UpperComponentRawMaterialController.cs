using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    class UpperComponentRawMaterialController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();
        public static bool Insert(UpperComponentRawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @UpperComponentID = new SqlParameter("@UpperComponentID", model.UpperComponentID);
            var @ETD = new SqlParameter("@ETD", model.ETD);

            if (db.ExecuteStoreCommand("EXEC spm_InsertUpperComponentRawMaterial @ProductNo, @UpperComponentID, @ETD", @ProductNo, @UpperComponentID, @ETD) > 0)
            {
                return true;
            }
            return false;
        }

        public static List<UpperComponentRawMaterialModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<UpperComponentRawMaterialModel>("EXEC spm_SelectUpperComponentRawMaterialByProductNo @ProductNo", @ProductNo).ToList();
        }

        public static List<UpperComponentRawMaterialModel> Select()
        {
            return db.ExecuteStoreQuery<UpperComponentRawMaterialModel>("EXEC spm_SelectUpperComponentRawMaterial").ToList();
        }

        public static bool Delete(string productNo, int upperComponentID)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            var @UpperComponentID = new SqlParameter("@UpperComponentID", upperComponentID);

            if (db.ExecuteStoreCommand("EXEC spm_DeleteUpperComponentRawMaterial @ProductNo, @UpperComponentID", @ProductNo, @UpperComponentID) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool UpdateActualDate(UpperComponentRawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @UpperComponentID = new SqlParameter("@UpperComponentID", model.UpperComponentID);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);

            if (db.ExecuteStoreCommand("EXEC spm_UpdateUpperComponentRawMaterialActualDate @ProductNo, @UpperComponentID, @ActualDate", @ProductNo, @UpperComponentID, @ActualDate) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsFull(List<SizeRunModel> sizeRunList, List<UpperComponentRawMaterialModel> upperComponentRawMaterialList, List<UpperComponentMaterialModel> upperComponentMaterialList)
        {
            foreach (UpperComponentRawMaterialModel upperComponentRawMaterial in upperComponentRawMaterialList)
            {
                foreach (SizeRunModel sizeRun in sizeRunList)
                {
                    //int quantity = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => (o.Quantity - o.QuantityReject));
                    int quantity = upperComponentMaterialList.Where(o => o.UpperComponentID == upperComponentRawMaterial.UpperComponentID && o.SizeNo == sizeRun.SizeNo).Sum(o => (o.Quantity));
                    if (quantity < sizeRun.Quantity)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
