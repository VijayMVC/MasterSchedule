using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using MasterSchedule.Models;
using MasterSchedule.Entities;

namespace MasterSchedule.Controllers
{
    public class InsockRawMaterialController
    {
        private static SaovietMasterScheduleEntities db = new SaovietMasterScheduleEntities();

        public static bool Insert(InsockRawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @InsockSupplierId = new SqlParameter("@InsockSupplierId", model.InsockSupplierId);
            var @ETD = new SqlParameter("@ETD", model.ETD);

            if (db.ExecuteStoreCommand("EXEC spm_InsertInsockRawMaterial @ProductNo, @InsockSupplierId, @ETD", @ProductNo, @InsockSupplierId, @ETD) > 0)
            {
                return true;
            }
            return false;
        }

        public static List<InsockRawMaterialModel> Select()
        {
            return db.ExecuteStoreQuery<InsockRawMaterialModel>("EXEC spm_SelectInsockRawMaterial").ToList();
        }

        public static List<InsockRawMaterialModel> SelectPerPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<InsockRawMaterialModel>("EXEC spm_SelectInsockRawMaterialByProductNo @ProductNo", @ProductNo).ToList();
        }

        public static bool Delete(string productNo, int insockSupplierId)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            var @InsockSupplierId = new SqlParameter("@InsockSupplierId", insockSupplierId);

            if (db.ExecuteStoreCommand("EXEC spm_DeleteInsockRawMaterial @ProductNo, @InsockSupplierId", @ProductNo, @InsockSupplierId) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool UpdateActualDate(InsockRawMaterialModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @InsockSupplierId = new SqlParameter("@InsockSupplierId", model.InsockSupplierId);
            var @ActualDate = new SqlParameter("@ActualDate", model.ActualDate);

            if (db.ExecuteStoreCommand("EXEC spm_UpdateInsockRawMaterialActualDate @ProductNo, @InsockSupplierId, @ActualDate", @ProductNo, @InsockSupplierId, @ActualDate) > 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsFull(List<SizeRunModel> sizeRunList, List<InsockRawMaterialModel> insockRawMaterialList, List<InsockMaterialModel> insockMaterialList)
        {
            foreach (InsockRawMaterialModel insockRawMaterial in insockRawMaterialList)
            {
                foreach (SizeRunModel sizeRun in sizeRunList)
                {
                    //int quantity = outsoleMaterialList.Where(o => o.OutsoleSupplierId == outsoleRawMaterial.OutsoleSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => (o.Quantity - o.QuantityReject));
                    int quantity = insockMaterialList.Where(o => o.InsockSupplierId == insockRawMaterial.InsockSupplierId && o.SizeNo == sizeRun.SizeNo).Sum(o => (o.Quantity));
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
